using System;
using Common.BothWayChainList;

namespace DataStruct.HashTable.HashTable_ChainList
{
    public class HashTable_ChainList<TKey,TValue>
    {
        #region 数据
        //哈希链桶
        private BothWayChainList<HashBucket<TKey,TValue>> [] buckets;
        private const int MaxChainCount = 10;
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
        }
        //实时记录的当前最长的链表长度
        private int maxCahinCount;
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
        public HashTable_ChainList()
        {
            buckets = new BothWayChainList<HashBucket<TKey, TValue>>[4];
        }
        #endregion
        
        #region 功能
        public void TryAdd(TKey key,TValue value)
        {
            CheckChangeSize();
            Add(key,value,buckets);
        }
        
        public void Add(TKey key,TValue value,BothWayChainList<HashBucket<TKey,TValue>>[] bucketChainList)
        {
            int hashIndex = HashTableKeyAlgorithm.LinearProbing(key, bucketChainList.Length);
            if(bucketChainList[hashIndex] == null)
            {
                bucketChainList[hashIndex] = new BothWayChainList<HashBucket<TKey, TValue>>();
            }
            bucketChainList[hashIndex].Append(new HashBucket<TKey, TValue>(key,value));
            maxCahinCount = Math.Max(bucketChainList[hashIndex].Count, maxCahinCount);
            count++;
        }
        
        public void TryRemove(TKey key)
        {
            int hashIndex = HashTableKeyAlgorithm.LinearProbing(key, buckets.Length);
            BothWayChainList<HashBucket<TKey, TValue>> chainList = buckets[hashIndex];
            if(chainList == null)
            {
                return;
            }

            int index = 0;
            foreach(HashBucket<TKey,TValue> node in chainList)
            {
                if(node.KeyIsEqual(key))
                {
                    node.Delete();
                    chainList.Delete(index);
                    maxCahinCount = Math.Max(chainList.Count, maxCahinCount);
                    count--;
                    return;
                }
                index++;
            }
        }
        
        public void Clear()
        {
            Array.Clear(buckets,0,buckets.Length);
            count = 0;
        }
        
        public bool ContainKey(TKey key)
        {
            TValue value = GetValue(key);
            return !value.Equals(default(TValue));
        }
        
        public bool ContainValue(TValue value)
        {
            foreach(var chainList in buckets)
            {
                foreach(HashBucket<TKey, TValue> node in chainList)
                {
                    if(node.ValueIsEqual(value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public TValue GetValue(TKey key)
        {
            int hashIndex = HashTableKeyAlgorithm.LinearProbing(key, buckets.Length);
            BothWayChainList<HashBucket<TKey, TValue>> chainList = buckets[hashIndex];
            if(chainList == null)
            {
                return default(TValue);
            }
            foreach(HashBucket<TKey, TValue> node in chainList)
            {
                if(node.KeyIsEqual(key))
                {
                    return node.Value;
                }
            }

            return default(TValue);
        }
        
        /// <summary>
        /// 变更容器大小，扩容（缩容）
        /// </summary>
        public void CheckChangeSize()
        {
            //扩容
            if(maxCahinCount >= MaxChainCount)
            {
                BothWayChainList<HashBucket<TKey, TValue>>[] newBuckets = new BothWayChainList<HashBucket<TKey, TValue>>[buckets.Length * 2];
                foreach(var chainList in buckets)
                {
                    if(chainList == null)
                    {
                        continue;
                    }
                    foreach(HashBucket<TKey,TValue> node in chainList)
                    {
                        Add(node.Key,node.Value,newBuckets);
                    }
                }

                buckets = newBuckets;
            }
        }
        #endregion
    }
}