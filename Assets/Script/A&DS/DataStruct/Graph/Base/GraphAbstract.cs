using Common.DataStruct.Queue.ChainQueue;

namespace DataStruct.Graph.Base
{
    public enum GraphType
    {
        UndirectedGraph,
        DirectedGraph,        
    }
    
    public abstract class GraphAbstract<T>
    {
        #region 数据
        //图类型
        protected GraphType type;
        //节点数量，边数量
        protected int vertexNum, edgeNum;
        //顶点列表
        protected NodeAbstract<T>[] vertexs;
        public virtual NodeAbstract<T>[] Vertexs
        {
            get
            {
                return vertexs;
            }
        }
        #endregion

        #region 构造
        public GraphAbstract(GraphType type)
        {
            vertexs = new NodeAbstract<T>[5];
            vertexNum = edgeNum = 0;
            this.type = type;
        }
        #endregion
        
        #region 顶点
        /// <summary>
        /// 添加顶点
        /// </summary>
        /// <param name="data"></param>
        public void AddNode(T data)
        {
            if(vertexNum == vertexs.Length)
                Expand();
            vertexs[vertexNum++] = GetNode(data);
        }
        
        /// <summary>
        /// 删除顶点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public T RemoveNode(T data)
        {
            return RemoveNode(GetIndex(data));
        }
        #endregion
        
        #region 边
        /// <summary>
        /// 图添加边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="len"></param>
        public void GraphAddEdge(int start,int end,int len)
        {
            if(type == GraphType.DirectedGraph)
                AddArc(start,end,len);
            else
                AddEdge(start,end,len);
        }
        
        /// <summary>
        /// 图删除边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void GraphRemoveEdge(int start,int end)
        {
            if(type == GraphType.DirectedGraph)
                RemvoeArc(start,end);
            else
                RemvoeEdge(start,end);           
        }
        #endregion
        
        #region 获取
        /// <summary>
        /// 获取点位置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int GetIndex(T data)
        {
            for(int i = 0;i < vertexNum;i++)
            {
                if(data.Equals(vertexs[i].data))
                {
                    return i;
                }   
            }
            
            return -1;
        }
        
        /// <summary>
        /// 获取节点数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetData(int index)
        {
            NodeAbstract<T> node = GetNode(index);
            return node == null ? default(T) : node.data;
        }
        
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NodeAbstract<T> GetNode(int index)
        {
            if(index < 0 || index >= vertexNum)
            {
                return null;
            }

            return Vertexs[index];
        }
        #endregion
        
        #region 数据清理
        public void ClearNodeVisit()
        {
            if(vertexNum <= 0)
            {
                return;
            }
            for(int i = 0;i < vertexNum;i++)
            {
                vertexs[i].isVisit = false;
            }
        }
        #endregion
        
        #region 广度优先遍历
        /// <summary>
        /// 广度优先遍历（包含连通分量）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ChainQueue<NodeAbstract<T>> GraphBFS(int index)
        {
            ClearNodeVisit();
            ChainQueue<NodeAbstract<T>> queue = BFSorder(index);
            for(int i = 0;i < vertexNum;i++)
            {
                //连通分量构建树
                if(vertexs[i].isVisit == false)
                {
                    queue += BFSorder(i);
                }
            }

            return queue;
        }
        
        /// <summary>
        /// 广度优先遍历（单树）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ChainQueue<NodeAbstract<T>> SingleBFS(int index)
        {
            ClearNodeVisit();
            return BFSorder(index);
        }
        #endregion
        
        #region 深度优先遍历
        /// <summary>
        /// 深度优先遍历（包含连通分量）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ChainQueue<NodeAbstract<T>> GraphDFS(int index)
        {
            ClearNodeVisit();
            ChainQueue<NodeAbstract<T>> queue = DFSorder(index);
            for(int i = 0;i < vertexNum;i++)
            {
                //连通分量构建树
                if(vertexs[i].isVisit == false)
                {
                    queue += DFSorder(i);
                }
            }

            return queue;
        }
        
        /// <summary>
        /// 单个节点深度优先遍历
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ChainQueue<NodeAbstract<T>> SingleDFS(int index)
        {
            ClearNodeVisit();
            return DFSorder(index);
        }
        #endregion
        
        #region 生成最小树
        /// <summary>
        /// 获取最小生成树
        /// </summary>
        /// <param name="index">起点顶点</param>
        /// <returns></returns>
        public Edge[] GetEdges(int index)
        {
            ClearNodeVisit();
            Edge[] edges = new Edge[vertexNum - 1];
            vertexs[index].isVisit = true;
            for (int i = 0;i < edges.Length;i++)
            {
                edges[i] = GetMinEdge();
            }

            return edges;
        }
        #endregion
        
        #region 接口
        public abstract void Expand();
        public abstract NodeAbstract<T> GetNode(T data);
        public abstract T RemoveNode(int index);
        public abstract void AddEdge(int start, int end, int len);
        public abstract void AddArc(int start, int end, int len);
        public abstract void RemvoeEdge(int start, int end);
        public abstract void RemvoeArc(int start, int end);

        public virtual int GetEdgeWeight(int start, int end)
        {
            return -1;
        }

        public abstract ChainQueue<NodeAbstract<T>> BFSorder(int index);
        public abstract ChainQueue<NodeAbstract<T>> DFSorder(int index);
        public abstract Edge GetMinEdge();
        #endregion
    }
}