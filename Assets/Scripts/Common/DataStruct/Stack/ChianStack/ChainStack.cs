namespace Common.DataStruct.Stack.ChianStack
{
    public class ChainStack<T>
    {
        #region 数据
        public Node<T> Top { get; set; }
        public int Count { get; set; }
        #endregion
        
        #region 构造
        public ChainStack()
        {
            Top = null;
            Count = 0;
        }
        
        public ChainStack(T[] array)
        {
            if(array == null || array.Length <= 0)
            {
                return;
            }
            foreach(T item in array)
            {
                Push(item);
            }
        }
        #endregion
        
        #region 查询
        public bool IsEmpty()
        {
            return Top == null || Count == 0;
        }
        #endregion
        
        #region 功能
        public void Push(T item)
        {
            Node<T> node = new Node<T>(item);
            if(Top == null)
            {
                Top = node;
            }
            else
            {
                node.Next = Top;
                Top = node;
            }

            Count++;
        }
        
        public T Pop()
        {
            if(IsEmpty())
            {
                return default(T);
            }

            Node<T> node = Top;
            Top = Top.Next;
            Count--;
            return node.Data;
        }
        #endregion
    }
}