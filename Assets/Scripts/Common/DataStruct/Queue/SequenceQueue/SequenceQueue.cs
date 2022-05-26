using System;

namespace Common.DataStruct.Queue.SequenceQueue
{
    public class SequenceQueue<T>
    {
        #region 数据

        private T[] items;
        private T[] Items
        {
            get
            {
                if(items == null)
                {
                    items = new T[4];
                }

                return items;
            }
        }

        private int count;
        public int Count
        {
            get
            {
                return count;
            }
        }

        private int head;
        private int tail;
        #endregion
        
        #region 构造
        public SequenceQueue()
        {
            items = new T[4];
        }
        public SequenceQueue(int count)
        {
            items = new T[count];
        }
        
        public SequenceQueue(T[] array)
        {
            if(array == null || array.Length == 0)
            {
                return;
            }

            items = new T[array.Length];
            foreach(var item in array)
            {
                Enqueue(item);
            }
        }
        #endregion
        
        #region 功能
        public void Enqueue(T item)
        {
            if(Count >= Items.Length)
            {
                Dilatation();
            }

            items[tail] = item;
            tail = (tail + 1) % Items.Length;
            count++;
        }
        
        public T Dequeue()
        {
            if(Count == 0)
            {
                return default(T);
            }

            T result = Items[head];
            Items[head] = default(T);
            head = (head + 1) % Items.Length;
            count--;
            return result;
        }
        
        private void Dilatation()
        {
            T[] newItems = new T[items.Length * 2];
            if(Count > 0)
            {
                if(head < tail)
                {
                    Array.Copy(items,head,newItems,0,Count);
                }
                else
                {
                    Array.Copy(items,head,newItems,0,items.Length - head);
                    Array.Copy(items,0,newItems,items.Length - head,tail);
                }
            }

            head = 0;
            tail = Count;
            items = newItems;
        }
        #endregion
    }
}