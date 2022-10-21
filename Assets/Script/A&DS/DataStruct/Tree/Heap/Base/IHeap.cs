namespace DataStruct.Tree.Heap
{
    public interface IHeap<T>
    {
        int Count
        {
            get;
        }
        void Push(T data);
        T RemoveByIndex(int index);
        bool Remove(T data);
        bool Find(T data);
        int FindIndex(T data);
    }
}