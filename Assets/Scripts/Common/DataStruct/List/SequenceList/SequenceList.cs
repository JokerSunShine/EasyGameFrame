using Renci.SshNet.Security;

namespace Common.SequenceList
{
    public class SequenceList<T>:IList<T>
    {
        #region 数据
        private T[] items;

        private int size;
        
        //索引器
        public T this[int index]
        {
            get
            {
                return GetElem(index);
            }
            set
            {
                Insert(value,index);
            }
        }
        
        public int Count
        {
            get
            {
                return size;
            }
        }
        #endregion

        #region 构造
        public SequenceList(int size)
        {
            items = new T[size];
            this.size = size;
        }
        
        public SequenceList(T[] curItems)
        {
            items = curItems;
            size = items.Length;
        }
        #endregion
        
        public void Clear()
        {
            size = 0;
        }
        
        public bool IsEmpty()
        {
            return size <= 0;
        }
        
        public void Append(T item)
        {
            if(size >= items.Length)
            {
                Dilatation();
            }

            items[size++] = item;
        }
        
        public void Insert(T item,int index)
        {
            if(size >= items.Length)
            {
                Dilatation();
            }
            if(index > size)
            {
                Append(item);
            }
            else
            {
                for(int i = size;i > index;i--)
                {
                    items[i] = items[i - 1];
                }

                items[index] = item;
                size++;
            }
        }
        
        public void Delete(int index)
        {
            if(index >= size)
            {
                return;
            }
            for(int i = index;i < size;i++)
            {
                items[i] = items[i + 1];
            }

            size--;
        }
        
        public T GetElem(int index)
        {
            if(items == null || index >= items.Length)
            {
                return default(T);
            }

            return items[index];
        }
        
        public int locate(T value)
        {
            if(IsEmpty())
            {
                return -1;
            }
            for(int i = 0;i < items.Length;i++)
            {
                if(items[i].Equals(value))
                {
                    return i;
                }
            }

            return -1;
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
        
        public void ReverList()
        {
            if(size <= 0)
            {
                return;
            }
            T temp = default(T);
            int maxIndex = size - 1;
            for(int i = 0;i < size / 2;i++)
            {
                temp = items[i];
                items[i] = items[maxIndex - i];
                items[maxIndex - i] = temp;
            }
        }
        
        public void Merge(SequenceList<T> mergeList)
        {
            if(mergeList == null)
            {
                return;
            }
            int i = 0;
            while(i < mergeList.Count)
            {
                this.Append(mergeList[i++]);
            }
        }
    }
}