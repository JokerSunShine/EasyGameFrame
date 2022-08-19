namespace DataStruct.HashTable
{
    public class HashBucket<TKey,TValue>
    {
        #region 枚举
        public enum BucketState
        {
            Empty,
            Occupy
        }
        #endregion
        
        #region 数据
        private TKey key;
        public TKey Key
        {
            get
            {
                return key;
            }
            set
            {
                KeyHashCode = value == null || value.Equals(default(TKey)) ? 0 : Key.GetHashCode();
                key = value;
            }
        }
        private TValue _value;
        public TValue Value
        {
            get
            {
                return _value;
            }
            set
            {
                if(value == null || value.Equals(default(TValue)))
                {
                    ValueHashCode = 0;
                    state = BucketState.Empty;
                }
                else
                {
                    ValueHashCode = value.GetHashCode();
                    state = BucketState.Occupy;
                }
                _value = value;
            }
        }

        public int KeyHashCode;
        public int ValueHashCode;
        public int Next;
        public BucketState state;
        #endregion
        
        #region 构造
        public HashBucket(TKey key,TValue value)
        {
            RefreshValue(key, value);
        }

        public void RefreshValue(TKey key,TValue value)
        {
            this.Key = key;
            this.Value = value;
            state = BucketState.Occupy;
        }
        #endregion
        
        #region 功能
        public bool KeyIsEqual(TKey key)
        {
            return key.GetHashCode() == KeyHashCode && Key.Equals(key);
        }
        
        public bool ValueIsEqual(TValue value)
        {
            return value.GetHashCode() == ValueHashCode && Value.Equals(value);
        }
        public void Delete()
        {
            Key = default(TKey);
            Value = default(TValue);
            KeyHashCode = 0;
            ValueHashCode = 0;
            state = BucketState.Empty;
        }
        #endregion
    }
}