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
        #endregion
        
        #region 功能
        public void Enqueue(T item)
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