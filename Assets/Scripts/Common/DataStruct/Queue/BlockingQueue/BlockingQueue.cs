using System.Threading;
using Common.DataStruct.Queue.SequenceQueue;

namespace Common.DataStruct.Queue.BlockingQueue
{
    public class BlockingQueue<T>
    {
        private readonly SequenceQueue<T> queue = new SequenceQueue<T>();
        private readonly int maxSize;
        private bool closing;
        public int Count
        {
            get
            {
                return queue.Count;
            }
        }
        
        #region 构造
        public BlockingQueue(int maxSize)
        {
            this.maxSize = maxSize;
        }
        #endregion
        
        #region 功能
        public void Enqueue(T item)
        {
            lock(queue)
            {
                while(queue.Count >= maxSize)
                {
                    //到承载最大数量，则线程放到等待队列挂起
                    Monitor.Wait(queue);
                }
                
                CSDebug.Log("+++++++添加数据" + item);
                queue.Enqueue(item);
                if(queue.Count == 1)
                {
                    //有数据后，通知等待队列的所有线程可以去争夺queue
                    Monitor.PulseAll(queue);
                }
            }
        }
        
        public bool TryDequeue(out T value)
        {
            lock(queue)
            {
                while(queue.Count == 0)
                {
                    value = default(T);
                    return false; 
                }
                value = queue.Dequeue();
                CSDebug.Log("------提取数据" + value);
                if(queue.Count == maxSize - 1)
                {
                    //提取了一个脉冲通知等待队列所有线程到就绪队列争夺queue
                    Monitor.PulseAll(queue);
                }

                return true;
            }
        }
        
        public void Close()
        {
            lock(queue)
            {
                closing = true;
                Monitor.PulseAll(queue);
            }
        }
        #endregion
    }
}