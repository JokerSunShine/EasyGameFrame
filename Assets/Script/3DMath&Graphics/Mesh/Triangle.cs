using _3DMath;

namespace Mesh
{
    //三角形
    public class Triangle
    {   
        #region 数据
        private bool isInit = false;
        private Vertex[] vertex = new Vertex[3];
        private Vector3 normal;
        public Vector3 Normal
        {
            get
            {
                if(normal == null)
                {
                    normal = ComputeTriangleNormal(this);
                }

                return normal;
            }
        }
        #endregion
        
        #region 构造
        public Triangle()
        {
            SetDefault();
        }
        #endregion
        
        #region 内部
        public void SetDefault()
        {
            normal = Vector3.zero;
            if(isInit)
            {
                foreach(var v in vertex)
                {
                    v.SetDefualt();
                }
            }
            else
            {
                for(int i = 0;i < 3;i++)
                {
                    vertex[i] = new Vertex();
                    vertex[i].SetDefualt();
                }
            }
        }
        public bool IsValid()
        {
            return vertex.Length > 2 && isInit;
        }
        
        public Vertex GetVertex(int index)
        {
            if(vertex == null)
            {
                return null;
            }
            return vertex[index];
        }
        
        public Vector3 GetVertexPosition(int index)
        {
            if(vertex == null)
            {
                return null;
            }
            return vertex[index].p;
        }
        #endregion
        
        #region 静态
        public static Vector3 ComputeTriangleNormal(Triangle t)
        {
            if(t == null || t.IsValid() == false)
            {
                return null;
            }

            Vector3 v1 = t.GetVertexPosition(0);
            Vector3 v2 = t.GetVertexPosition(1);
            Vector3 v3 = t.GetVertexPosition(2);

            Vector3 e3 = v2 - v1;
            Vector3 e1 = v3 - v2;
            return e3.Cross(e1).Normalize();
        }
        #endregion
    }
}