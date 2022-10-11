namespace DataStruct.Graph.ALGraph
{
    public class EdgeNode
    {
        #region 数据
        public int vertexIndex;
        public EdgeNode next;
        public int len;
        #endregion
        
        #region 构造
        public EdgeNode(int vertexIndex,int len)
        {
            this.vertexIndex = vertexIndex;
            this.len = len;
        }
        #endregion
        
        #region 设置
        /// <summary>
        /// 设置下一个边
        /// </summary>
        /// <param name="vertexIndex"></param>
        public void SetNext(int vertexIndex,int len)
        {
            next = new EdgeNode(vertexIndex,len);
        }
        #endregion
    }
}