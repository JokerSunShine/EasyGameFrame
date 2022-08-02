namespace Common.DataStruct.Queue.ChainQueue
{
    public class Node<T>
    {
        #region 数据

        private T item;
        public T Item
        {
            get
            {
                return item;
            }
        }

        private Node<T> next;
        public Node<T> Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }
        #endregion
        
        #region 构造
        public Node(T item)
        {
            this.item = item;
        }
        #endregion
    }
}