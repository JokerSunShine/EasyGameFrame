namespace Mesh
{
    //网格
    public class TriangleMesh
    {
        #region 数据
        private Vertex[] vertices;
        private Triangle[] triList;
        #endregion
        
        #region 内部
        public bool IsViald()
        {
            return vertices != null && vertices.Length > 0 && triList != null && triList.Length > 0;
        }
        #endregion
        
        #region 静态
        /// <summary>
        /// 计算所有顶点法向量，相邻平面法向量的平均值
        /// </summary>
        public static void CompoteVertexsNormal(TriangleMesh triangeMesh)
        {
            if(triangeMesh == null || triangeMesh.IsViald() == false)
            {
                return;
            }

            Vertex[] vertices = triangeMesh.vertices;
            Triangle[] triList = triangeMesh.triList;
            foreach(Vertex v in vertices)
            {
                v.ResetNormal();    
            }
            
            foreach(var t in triList)
            {
                for(int i = 0;i < 3;i++)
                {
                    t.GetVertex(i).normal += t.Normal;
                }
            }
            
            foreach(var v in vertices)
            {
                v.normal.Normalize();
            }
        }
        #endregion
    }
}