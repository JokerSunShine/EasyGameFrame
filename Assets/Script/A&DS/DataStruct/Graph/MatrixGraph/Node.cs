namespace DataStruct.Graph.MatrixGraph
{
    public class Node<T>
    {
        #region 数据
        /// <summary>
        /// 元数据
        /// </summary>
        public T data;
        /// <summary>
        /// 查询状态
        /// </summary>
        public bool IsVisited;
        #endregion
        
        #region 构造
        public Node(T data)
        {
            this.data = data;
            IsVisited = false;
        }
        #endregion
        
        #region 重置数据
        public void ResetState()
        {
            IsVisited = false;
        }
        #endregion
    }
}