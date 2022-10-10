namespace DataStruct.Graph.MatrixGraph
{
    public class Edge
    {
        #region 数据
        //起点下标
        public int start;
        //终点下标
        public int end;
        //权重
        public int len;
        #endregion
        
        #region 构造
        public Edge(int start,int end,int len)
        {
            this.start = start;
            this.end = end;
            this.len = len;
        }
        #endregion
    }
}