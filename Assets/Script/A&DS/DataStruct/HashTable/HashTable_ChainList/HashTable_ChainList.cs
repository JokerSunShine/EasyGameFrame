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
        #endregion
        
        #region 构造
        public HashTable_ChainList(){}
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
        }
        
        public void TryRemove(TKey key)
        {
            int hashIndex = HashTableKeyAlgorithm.LinearProbing(key, buckets.Length);
            BothWayChainList<HashBucket<TKey, TValue>> chainList = buckets[hashIndex];
            if(chainList == null)
            {
                return;
            }
            foreach(HashBucket<TKey,TValue> node in chainList)
            {
                if(node.KeyIsEqual(key))
                {
                    node.Delete();
                    return;
                }
            }
        }
        
        public void Clear()
        {
            Array.Clear(buckets,0,buckets.Length);
        }
        
        public bool ContainKey(TKey key)
        {
            return true;
        }
        
        public bool ContainValue(TValue value)
        {
            return true;
        }
        
        public TValue GetValue(TKey key)
        {
            return default(TValue);
        }
        
        public void CheckChangeSize()
        {
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