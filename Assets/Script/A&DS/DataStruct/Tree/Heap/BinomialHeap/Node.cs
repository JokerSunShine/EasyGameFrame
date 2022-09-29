namespace DataStruct.Tree.Heap.BinomialHeap
{
    public class Node<T>
    {
        public T data;
        public int degree;
        public Node<T> child;
        public Node<T> parent;
        public Node<T> next;
        
        public Node(T data)
        {
            this.data = data;
        }
    }
}