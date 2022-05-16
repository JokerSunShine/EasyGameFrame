namespace Common.BothWayChainList
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
            set
            {
                next = value;
            }
        }

        private Node<T> prev;
        public Node<T> Prev
        {
            get
            {
                return prev;
            }
            set
            {
                prev = value;
            }
        }
        #endregion
        
        #region 构造
        public Node()
        {
            data = default(T);
            next = null;
            prev = null;
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
            //没有数据表示当前为空头
            if(HaveData() == false)
            {
                data = item;
                return this;
            }
            //正常关联
            Node<T> originNext = Next;
            Node<T> newNext = new Node<T>(item);
            Next = newNext;
            newNext.Prev = this;
            if(originNext != null)
            {
                newNext.Next = originNext;
                originNext.Prev = newNext; 
            }
            return Next;
        }
        
        public Node<T> AddNext(Node<T> node,Node<T> prev)
        {
            if(data == null)
            {
                return null;
            }

            next = node;
            this.prev = prev;
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