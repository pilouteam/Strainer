using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;
using System.Linq;
using Cirrious.CrossCore.Exceptions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace Strainer.Http.Caching
{
    public class FileStore: ICacheStore
    {
        private const string CacheIndexFileName = "_CacheIndex.txt";
        private const string CacheFolder = "http";
        private const string CacheName = "http";
        private static readonly TimeSpan PeriodSaveInterval = TimeSpan.FromSeconds(1.0);

        private readonly IMvxFileStore _fileService;
        private readonly IMvxTextSerializer _serializer;
        private readonly Dictionary<string, Entry> _entriesByHttpUrl;
        private readonly Queue<string> _toDeleteFiles = new Queue<string>();

        private object _gate = new object();
        private bool _indexNeedsSaving;

        public FileStore(IMvxFileStore fileService, IMvxTextSerializer serializer)
        {
            this._serializer = serializer;
            _fileService = fileService;
           
            EnsureCacheFolderExists();
            _entriesByHttpUrl = LoadIndexEntries();

            QueueUnindexedFilesForDelete();
            QueueOutOfDateFilesForDelete();

            _indexNeedsSaving = false;

            Observable.Interval(PeriodSaveInterval)
                .Subscribe(_ => DoPeriodicTasks());
        }

        private string IndexFilePath
        {
            get
            {
                return _fileService.PathCombine(CacheFolder, CacheName + CacheIndexFileName);
            }
        }


        public byte[] Get(Uri url)
        {
            lock(_gate)
            {
                Entry diskEntry;
                byte[] contents;
                if (_entriesByHttpUrl.TryGetValue(url.ToString(), out diskEntry))
                {
                    if (diskEntry.ExpiresAtUtc < DateTime.UtcNow)
                    {
                        return null;
                    }
                    if (!_fileService.Exists(diskEntry.FilePath))
                    {
                        _entriesByHttpUrl.Remove(url.ToString());
                    }
                    else if(_fileService.TryReadBinaryFile(diskEntry.FilePath, out contents))
                    {
                        return contents;
                    }
                }
                return null;
            }
        }

        public void Set(Uri url, byte[] value, DateTime expiresAtUtc)
        {
            Entry diskEntry;
            lock (_gate)
            {
                if (!_entriesByHttpUrl.TryGetValue(url.ToString(), out diskEntry))
                {
                    var filePath = _fileService.PathCombine(CacheFolder, Guid.NewGuid().ToString("N"));
                    diskEntry = new Entry
                    {
                        FilePath = filePath,
                        HttpSource = url.ToString(),
                    };
                    _entriesByHttpUrl[url.ToString()] = diskEntry;
                }

                diskEntry.ExpiresAtUtc = expiresAtUtc;
                _indexNeedsSaving = true;
            

                _fileService.WriteFile(diskEntry.FilePath, value);
            }
        }

        private void DeleteNextUnneededFile()
        {
            string nextFileToDelete = "";
            lock(_gate)
            {
                if (_toDeleteFiles.Count == 0)
                {
                    return;
                }

                nextFileToDelete = _toDeleteFiles.Dequeue();
            };

            if (string.IsNullOrEmpty(nextFileToDelete))
            {
                return;
            }

            try
            {
                if (_fileService.Exists(nextFileToDelete))
                {
                    _fileService.DeleteFile(nextFileToDelete);
                }
            }
            catch (Exception exception)
            {
                MvxTrace.Warning( "Problem seen deleting file {0} problem {1}", nextFileToDelete,
                    exception.ToLongString());
            }
        }

        private void QueueOutOfDateFilesForDelete()
        {
            var now = DateTime.UtcNow;
            var toRemove = _entriesByHttpUrl
                .Values
                .Where(x => now > x.ExpiresAtUtc).ToList();

            foreach (var entry in toRemove)
            {
                _entriesByHttpUrl.Remove(entry.HttpSource);
            }
            lock (_gate)
            {
                foreach (var file in toRemove.Select(x => x.FilePath))
                {
                    _toDeleteFiles.Enqueue(file);
                }
            }
        }

        private void QueueUnindexedFilesForDelete()
        {
            var files = _fileService.GetFilesIn(CacheFolder);

            var cachedFiles = new Dictionary<string, Entry>();
            foreach (var e in _entriesByHttpUrl)
            {
                cachedFiles[e.Value.FilePath] = e.Value;
            }

            var toDelete = new List<string>();
            foreach (var file in files)
            {
                if (!cachedFiles.ContainsKey(file))
                if (!file.EndsWith(CacheIndexFileName))
                {
                    toDelete.Add(file);
                }
            }

            lock(_gate)
            {
                foreach (var file in toDelete)
                {
                    _toDeleteFiles.Enqueue(file);
                }
            };
        }

        private void EnsureCacheFolderExists()
        {
            _fileService.EnsureFolderExists(CacheFolder);
        }

        private Dictionary<string, Entry> LoadIndexEntries()
        {
            try
            {
                string text;
                if (_fileService.TryReadTextFile(IndexFilePath, out text))
                {
                    var list = _serializer.DeserializeObject<List<Entry>>(text);
                    return list.ToDictionary(x => x.HttpSource, x => x);
                }
            }
            catch (Exception exception)
            {
                MvxTrace.Warning( "Failed to read cache index {0} - reason {1}", CacheFolder,
                    exception.ToLongString());
            }

            return new Dictionary<string, Entry>();
        }



        private void DoPeriodicTasks()
        {
            SaveIndexIfDirty();
            DeleteNextUnneededFile();
        }

        private void SaveIndexIfDirty()
        {
            if (!_indexNeedsSaving)
                return;

            List<Entry> toSave = null;
            lock(_gate)
            {
                toSave = _entriesByHttpUrl.Values.ToList();
                _indexNeedsSaving = false;
            };

            try
            {
                var text = _serializer.SerializeObject(toSave);
                _fileService.WriteFile(IndexFilePath, text);
            }
            catch (Exception exception)
            {
                MvxTrace.Warning( "Failed to save cache index {0} - reason {1}", CacheFolder,
                    exception.ToLongString());
            }
        }

        public class Entry
        {
            public string HttpSource { get; set; }
            public string FilePath { get; set; }
            public DateTime ExpiresAtUtc { get; set; }
        }

    }
}

