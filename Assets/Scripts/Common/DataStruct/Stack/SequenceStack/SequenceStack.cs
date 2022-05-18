namespace Common.DataStruct.Stack.SequenceStack
{
    public class SequenceStack<T>
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
        #endregion
        
        #region 构造
        public SequenceStack(int count)
        {
            items = new T[count];
        }
        
        public SequenceStack(T[] array)
        {
            if(array == null || array.Length <= 0)
            {
                return;
            }
            foreach(T item in array)
            {
                Push(item);
            }
        }
        #endregion
        
        #region 功能
        public bool Push(T item)
        {
            if(count >= Items.Length)
            {
                Dilatation();
            }

            Items[count++] = item;
            return true;
        }
        
        public T Pop()
        {
            if(count == 0)
            {
                return default(T);
            }

            return Items[--count];
        }
        
        private void Dilatation()
        {
            T[] newItems = new T[items.Length * 2];
            for(int i = 0;i < items.Length;i++)
            {
                newItems[i] = items[i];
            }

            items = newItems;
        }
        #endregion
    }
}