namespace DataStruct.HashTable.HashTable_OpenAddress
{
    public class HashTable_OpenAddress<TKey,TValue>
    {
        #region 数据
        //哈希桶
        private HashBucket<TKey,TValue>[] buckets;
        //加载因子
        public float loadedValue;
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
        public HashTable_OpenAddress():this(4)
        {
        }
        
        public HashTable_OpenAddress(int count)
        {
            buckets = new HashBucket<TKey, TValue>[count];
            this.count = count;
            loadedValue = 0;
        }
        #endregion
        
        #region 功能
        public void Add(TKey key,TValue value)
        {
            
        }
        
        public void Remove(TKey key)
        {
            
        }
        
        public void Clear()
        {
            
        }
        
        public void ContainEey(TKey key)
        {
            
        }
        
        public void ContainValue(TValue value)
        {
            
        }
        
        public TValue GetValue(TKey key)
        {
            return default(TValue);
        }
        #endregion
    }
}