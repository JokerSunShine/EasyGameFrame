namespace Common.SequenceList
{
    public interface IList<T>
    {
        int Count
        {
            get;
        }

        void Clear();
        bool IsEmpty();
        void Append(T item);
        void Insert(T item,int index);
        void Delete(int index);
        T GetElem(int index);
        int locate(T value);
    }
}