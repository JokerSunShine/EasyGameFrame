namespace DataStruct.Queue.DoubleEndQueue.DoubleEndQueue_Chain
{
    public class DoubleEndQueue_Chain<T>
    {
        #region 数据
        private Node<T> head;
        private Node<T> tail;
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
        }
        #endregion
        
        #region 构造
        public DoubleEndQueue_Chain()
        {
            
        }
        
        public DoubleEndQueue_Chain(T[] array)
        {
            if(array == null || array.Length == 0)
            {
                return;
            }
            foreach(var item in array)
            {
                LastEnqueue(item);
            }
        }
        #endregion
        
        #region 功能
        public void FirstEnqueue(T item)
        {
            if(head == null)
            {
                head = tail = new Node<T>(item);
            }
            else
            {
                Node<T> newNode = new Node<T>(item);
                newNode.Next = head;
                head.Prev = newNode;
                head = newNode;
            }

            count++;
        }
        
        public void LastEnqueue(T item)
        {
            if(tail == null)
            {
                head = tail = new Node<T>(item);
            }
            else
            {
                Node<T> newNode = new Node<T>(item);
                newNode.Prev = tail;
                tail.Next = newNode;
                tail = newNode;
            }
            count++;
        }
        
        public T FirstDequeue()
        {
            if(IsEmpty())
            {
                return default(T);
            }

            T item = head.Item;
            head = head.Next;
            head.Prev = null;
            count--;
            return item;
        }
        
        public T LastDequeue()
        {
            if(IsEmpty())
            {
                return default(T);
            }

            T item = tail.Item;
            tail = tail.Prev;
            tail.Next = null;
            count--;
            return item;
        }
        #endregion
        
        #region 查询
        public bool IsEmpty()
        {
            return Count == 0 || head == null;
        }
        #endregion
    }
}