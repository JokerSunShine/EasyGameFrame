namespace DataStruct.Tree.Heap.FibHeap
{
    public class Node<T>
    {
        #region 数据
        public T data;
        public int degree;
        public Node<T> left;
        public Node<T> right;
        public Node<T> child;
        public Node<T> parent;
        //是否被删除第一个孩子
        public bool marked;
        #endregion

        #region 构造
        public Node(T data)
        {
            this.data = data;
        }
        #endregion
    }
}