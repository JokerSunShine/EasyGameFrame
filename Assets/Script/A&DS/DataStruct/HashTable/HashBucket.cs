namespace DataStruct.HashTable
{
    public class HashBucket<TKey,TValue>
    {
        public TKey key;
        public TValue value;
        public int hashCode;
        public int next;
    }
}