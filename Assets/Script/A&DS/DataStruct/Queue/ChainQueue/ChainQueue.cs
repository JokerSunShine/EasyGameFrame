using UnityEngine.UIElements;

namespace Common.DataStruct.Queue.ChainQueue
{
    public class ChainQueue<T>
    {
        #region 数据
        private Node<T> head;
        private Node<T> tail;
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
        }
        #endregion
        
        #region 构造
        public ChainQueue(){}
        public ChainQueue(T[] array)
        {
            if(array == null || array.Length == 0)
            {
                return;
            }
            foreach(var item in array)
            {
                Enqueue(item);
            }
        }
        
        public ChainQueue(ChainQueue<T> queue)
        {
            Node<T> nowNode = queue.head;
            if(queue.Count > 0)
            {
                while(nowNode != null)
                {
                    Enqueue(nowNode.Item);  
                    nowNode = nowNode.Next;
                }
            }
        }
        #endregion
        
        #region 功能
        public ChainQueue<T> Enqueue(T item)
        {
            if(tail == null)
            {
                tail = head = new Node<T>(item);
            }
            else
            {
                tail.Next = new Node<T>(item);
                tail = tail.Next;
            }
            count++;
            return this;
        }
        
        public T Dequeue()
        {
            if(IsEmpty())
            {
                return default(T);
            }
            T item = head.Item;
            head = head.Next;
            if(head == null)
            {
                tail = null;
            }
            count--;
            return item;
        }
        
        /// <summary>
        /// 将所有链式队列数据合并到一个新的队列中
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ChainQueue<T> CreateNewQueueByQueue(params ChainQueue<T>[] list)
        {
            if(list.Length <= 0)
            {
                return null;
            }
            ChainQueue<T> newQueue = new ChainQueue<T>();
            foreach(ChainQueue<T> queue in list)
            {
                if(queue != null)
                {
                    newQueue += queue;
                }
            }

            return newQueue;
        }
        #endregion
        
        #region 查询
        public bool IsEmpty()
        {
            return Count == 0 || head == null;
        }
        #endregion
        
        #region 静态功能
        //转移数据，加数组会被清空
        public static ChainQueue<T> operator+(ChainQueue<T> queue1,ChainQueue<T> queue2)
        {
            while(!queue2.IsEmpty())
            {
                queue1.Enqueue(queue2.Dequeue());
            }

            return queue1;
        }
        #endregion
    }
}