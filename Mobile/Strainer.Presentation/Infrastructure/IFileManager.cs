using System;
using System.Threading.Tasks;
using System.IO;

namespace Strainer.Infrastructure
{
    public interface IFileManager
    {
        bool FileExists(string path);

        bool DirectoryExists(string path);

        string Combine(params string[] paths);

        Task<string> Read(string path);

        Stream OpenRead(string path);

        Task Write(string path, string content);

        Task Write(string path, Stream content);

        Task DeleteFolder(string path, bool recursive = true);

        Task DeleteFile(string path);

        /// <summary>
        /// Try to fix the bug when accessing the special foder enum
        /// https://github.com/MvvmCross/MvvmCross/blob/3.2/Plugins/Cirrious/File/Cirrious.MvvmCross.Plugins.File.Touch/MvxTouchFileStore.cs
        /// and
        /// http://forums.xamarin.com/discussion/comment/82608/#Comment_82608
        /// </summary>
        string PersonalFolder {get;}
    }
}

