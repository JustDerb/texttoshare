using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace t2sBackend
{
    class BlockingQueue<T> : IEnumerable<T>
    {
        private int _count = 0;
        private Queue<T> _queue = new Queue<T>();

        public void Enqueue(T Obj)
        {
            if (Obj == null)
                throw new ArgumentNullException("Obj cannot be null");
            lock (_queue)
            {
                _queue.Enqueue(Obj);
                _count++;
                Monitor.Pulse(_queue);
            }
        }

        public T Dequeue()
        {
            lock (_queue)
            {
                while (_count <= 0) Monitor.Wait(_queue);
                _count--;
                return _queue.Dequeue();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (true) yield return Dequeue();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }
}
