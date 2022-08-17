using UnityEngine;

namespace DataStruct.Queue.DoubleEndQueue.DoubleEndQueue_Array
{
    public class DoubleEndQueue_Array<T>
    {
        #region 数据
        private T[] items;
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
        }

        private int front;
        private int rear;
        #endregion
        
        #region 构造
        public DoubleEndQueue_Array()
        {
            items = new T[4];
        }
        
        public DoubleEndQueue_Array(int count)
        {
            items = new T[count];
        }
        
        public DoubleEndQueue_Array(T[] array)
        {
            if(array == null || array.Length == 0)
            {
                return;
            }

            items = new T[array.Length];
            foreach(var item in array)
            {
                AddLast(item);
            }
        }
        #endregion
        
        #region 功能
        public void AddFrist(T item)
        {
            if(IsMax())
            {
                Dilatation();
            }
            
            front = items[front].Equals(default(T)) ? front : (front - 1 + items.Length) % items.Length;
            items[front] = item;
            count++;
        }
        public void AddLast(T item)
        {
            if(IsMax())
            {
                Dilatation();
            }
            rear = items[rear].Equals(default(T)) ? rear : (rear + 1) % items.Length;
            items[rear] = item;
            count++;
        }
        
        public T RemoveFirst()
        {
            T ret = items[front];
            front = (front + 1) % items.Length;
            count--;
            return ret;
        }
        
        public T RemoveLast()
        {
            T ret = items[rear];
            rear = (rear - 1 + items.Length) % items.Length;
            count--;
            return ret;
        }
        
        public T GetFirst()
        {
            return items[front];
        }
        
        public T GetLast()
        {
            return items[rear];
        }
        
        /// <summary>
        /// 扩容
        /// </summary>
        public void Dilatation()
        {
            T[] newItems = new T[items.Length * 2];
            int index = 0;
            for(index = 0;index < Count;index++)
            {
                newItems[index] = items[front++ % items.Length];
            }

            items = newItems;
            front = 0;
            rear = index - 1;
        }
        #endregion
        
        #region 查询
        /// <summary>
        /// 队列是否已满
        /// </summary>
        /// <returns></returns>
        private bool IsMax()
        {
            return Count >= items.Length;
        }
        #endregion
    }
}