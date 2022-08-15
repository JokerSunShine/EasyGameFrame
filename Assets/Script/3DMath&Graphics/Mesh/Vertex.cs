using _3DMath;

namespace Mesh
{
    //顶点
    public class Vertex
    {
        #region 数据
        public Vector3 p;
        public float u, v;
        public Vector3 normal;
        #endregion
        
        #region 构造
        public Vertex()
        {
            SetDefualt();
        }
        #endregion
        
        #region 内部
        public void SetDefualt()
        {
            p = Vector3.zero;
            normal = Vector3.zero;
            u = 0;
            v = 0;
        }
        
        public void ResetNormal()
        {
            normal = Vector3.zero;
        }
        #endregion
    }
}