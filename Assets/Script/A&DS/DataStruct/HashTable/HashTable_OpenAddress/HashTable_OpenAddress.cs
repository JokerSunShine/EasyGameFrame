using System;

namespace DataStruct.HashTable.HashTable_OpenAddress
{
    public class HashTable_OpenAddress<TKey,TValue>
    {
        #region 枚举
        //哈希函数类型
        public enum HashFuncType
        {
            LinearProbing,
            QuadraticProbing,
            DoubleHash
        }
        #endregion
        
        #region 数据
        //哈希桶
        private HashBucket<TKey,TValue>[] buckets;
        //加载因子最小值（用于动态控制内存）        
        private const float MinLoadedValue = 0.3f;
        //加载因子最大值（用于动态控制内存）      
        private const float MaxLoadedValue = 0.75f;
        //加载因子
        private float loadedValue;
        public float LoadedValue
        {
            get
            {
                return (float)Count / buckets.Length;
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

        private HashFuncType hashType;
        //索引器
        public TValue this[TKey index]
        {
            get
            {
                return GetValue(index);
            }
            set
            {
                if(value == null || value.Equals(default(TValue)))
                {
                    TryRemove(index);
                }
                else
                {
                    TryAdd(index,value);
                }
            }
        }

        #endregion
        
        #region 构造
        public HashTable_OpenAddress():this(4){}
        
        public HashTable_OpenAddress(HashFuncType type):this(4,type){}
        
        public HashTable_OpenAddress(int count,HashFuncType type = HashFuncType.DoubleHash)
        {
            buckets = new HashBucket<TKey, TValue>[count];
            this.hashType = type;
            loadedValue = 0;
        }
        #endregion
        
        #region 功能
        public void TryAdd(TKey key,TValue value)
        {
            CheckChangeSize();
            Add(key, value,buckets);
        }
        
        private void Add(TKey key,TValue value,HashBucket<TKey,TValue>[] buckets)
        {
            int newHashCode;
            int step = 0;
            while(step >= 0)
            {
                newHashCode = GetHashCode(key, buckets.Length, step, hashType);
                step++;
                HashBucket<TKey, TValue> bucket = buckets[newHashCode]; 
                if(bucket == null)
                {
                    buckets[newHashCode] =
                        new HashBucket<TKey, TValue>(key, value);
                    step = -1;
                    count++;
                }
                else if(bucket.state == HashBucket<TKey, TValue>.BucketState.Empty)
                {
                    bucket.RefreshValue(key,value);
                    step = -1;
                    count++;
                }
            }
        }
        
        public void TryRemove(TKey key)
        {
            int newHashCode;
            int step = 0;
            while(true)
            {
                newHashCode = GetHashCode(key, buckets.Length, 0, hashType);
                HashBucket<TKey, TValue> bucket = buckets[newHashCode]; 
                if(bucket == null || bucket.state == HashBucket<TKey, TValue>.BucketState.Empty)
                {
                    break;
                }
                step++;
                if(bucket.KeyIsEqual(key))
                {
                    count--;
                    bucket.Delete();
                    break;
                }
            }
        }
        
        public void Clear()
        {
            Array.Clear(buckets,0,buckets.Length);
            count = 0;
        }
        
        public bool ContainKey(TKey key)
        {
            foreach(var bucket in buckets)
            {
                if(bucket != null && bucket.KeyIsEqual(key))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool ContainValue(TValue value)
        {
            foreach(var bucket in buckets)
            {
                if(bucket != null && bucket.ValueIsEqual(value))
                {
                    return true;
                }
            }

            return false;
        }
        
        public TValue GetValue(TKey key)
        {
            int newHashCode;
            int step = 0;
            while(true)
            {
                newHashCode = GetHashCode(key, buckets.Length, 0, hashType);
                HashBucket<TKey, TValue> bucket = buckets[newHashCode]; 
                if(bucket == null || bucket.state == HashBucket<TKey, TValue>.BucketState.Empty)
                {
                    return default(TValue);
                }
                step++;
                if(bucket.KeyIsEqual(key))
                {
                    return bucket.Value;
                }
            }
        }
        
        /// <summary>
        /// 检测并修改容量
        /// </summary>
        private void CheckChangeSize()
        {
            float loaded = LoadedValue;
            //扩容
            if(loaded >= MaxLoadedValue)
            {
                count = 0;
                HashBucket<TKey, TValue>[] newBuckets = new HashBucket<TKey, TValue>[buckets.Length * 2];
                int newHashCode;
                foreach(var bucket in buckets)
                {
                    if(bucket == null || bucket.state == HashBucket<TKey, TValue>.BucketState.Empty)
                    {
                        continue;
                    }
                    Add(bucket.Key,bucket.Value,newBuckets);
                }

                buckets = newBuckets;
            }
        }
        
        public int GetHashCode(TKey key,int length,int step,HashFuncType type)
        {
            switch (type)
            {
                case HashFuncType.LinearProbing:
                    return HashTableKeyAlgorithm.LinearProbing(key,length,step);
                case HashFuncType.QuadraticProbing:
                    return HashTableKeyAlgorithm.QuadraticProbing(key,length,step);
                case HashFuncType.DoubleHash:
                    return HashTableKeyAlgorithm.DoubleHash(key,length,step);
            }

            return key.GetHashCode();
        }
        #endregion
    }
}