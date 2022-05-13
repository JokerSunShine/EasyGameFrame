namespace Common.OneWayLoopChainList
{
    public class Node<T>
    {
        #region 数据

        private T data;
        public T Data
        {
            get
            {
                return data;
            }
        }

        private Node<T> next;
        public Node<T> Next
        {
            get
            {
                return next;
            }
        }
        #endregion
        
        #region 构造
        public Node()
        {
            data = default(T);
            next = null;
        }
        
        public Node(T item)
        {
            data = item;
            next = null;
        }
        #endregion
        
        public Node<T> AddNext(T item)
        {
            if(item == null)
            {
                return null;
            }
            if(HaveData() == false)
            {
                data = item;
                return this;
            }
            Node<T> 
            next = new Node<T>(item);
            return next;
        }
        
        public Node<T> AddNext(Node<T> node)
        {
            if(data == null)
            {
                return null;
            }

            next = node;
            return node;
        }
        
        public bool HaveData()
        {
            return Data != null;
        }
        
        public bool HaveNextData()
        {
            return Next != null;
        }
    }
}