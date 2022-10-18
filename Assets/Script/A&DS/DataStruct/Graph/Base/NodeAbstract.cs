using System.Collections;

namespace DataStruct.Graph.Base
{
    public abstract class NodeAbstract<T>
    {
        #region 数据
        /// <summary>
        /// 节点元数据
        /// </summary>
        public T data;
        /// <summary>
        /// 权重
        /// </summary>
        public int len;
        /// <summary>
        /// 访问状态
        /// </summary>
        public bool isVisit;
        #endregion
        
        #region 构造
        public NodeAbstract(T data)
        {
            this.data = data;
            len = 0;
            isVisit = false;
        }
        #endregion
        
        #region 重置数据
        public void ResetState()
        {
            isVisit = false;
        }
        #endregion
        
        #region 虚方法
        public virtual void AddEdge(int vertexIndex,int len){}
        public virtual void RemoveEdge(int index){}
        #endregion
    }
}