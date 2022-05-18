namespace Common.DataStruct.Stack.ChianStack
{
    public class Node<T>
    {
        public T Data;
        public Node<T> Next;
        public Node(T item)
        {
            Data = item;
        }
    }
}