using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Strainer.Collections
{
    public class FixedSizedQueue<T>: IEnumerable<T>
    {
        ConcurrentQueue<T> _innerQueue = new ConcurrentQueue<T>();

        public FixedSizedQueue(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; private set; }

        public void Enqueue(T obj)
        {
            _innerQueue.Enqueue(obj);
            lock (this)
            {
                T overflow;
                while (_innerQueue.Count > Limit && _innerQueue.TryDequeue(out overflow)) ;
            }
        }

        public bool TryDequeue(out T result)
        {
            return _innerQueue.TryDequeue(out result);
        }

        public bool TryPeek(out T result)
        {
            return _innerQueue.TryPeek(out result);
        }

        public void Clear()
        {
            _innerQueue = new ConcurrentQueue<T>();
        }

        public int Count{ get { return _innerQueue.Count; } }
        public bool IsEmpty { get { return _innerQueue.IsEmpty; } }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerQueue.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _innerQueue.GetEnumerator();
        }
    }
}

