namespace DataStruct.Tree.BinaryTree
{
    public class Node<T>
    {
        #region 构造
        private T data;
        public T Data
        {
            get => data;
        }
        private Node<T> leftNode;
        public Node<T> LeftNode { get => leftNode; set => leftNode = value; }
        private Node<T> rightNode;
        public Node<T> RightNode { get => rightNode; set => rightNode = value; }
        private Node<T> parent;
        public Node<T> Parent
        {
            get => parent;
            set => parent = value;
        }
        
        /// <summary>
        /// 度
        /// </summary>
        public int Degree
        {
            get
            {
                int degree = 0;
                if(LeftNode != null)
                {
                    degree++;
                }
                if(rightNode != null)
                {
                    degree++;
                }

                return degree;
            }
        }
        #endregion
        
        #region 构造
        public Node()
        {
            data = default(T);
            leftNode = null;
            rightNode = null;
            parent = null;
        }
        
        public Node(T data,Node<T> leftNode = null,Node<T> rightNode = null,Node<T> parent = null)
        {
            this.data = data;
            this.leftNode = leftNode;
            this.rightNode = rightNode;
            this.parent = parent;
        }
        #endregion
        
        #region 功能
        public bool IsEqual(T data)
        {
            return this.data.Equals(data);
        }
        #endregion
    }
}