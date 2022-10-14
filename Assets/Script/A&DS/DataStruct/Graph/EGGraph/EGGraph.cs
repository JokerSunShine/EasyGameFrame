using Common.DataStruct.Queue.ChainQueue;
using Common.OneWayChainList;
using DataStruct.Graph.Base;

namespace DataStruct.Graph.EGGraph
{
    //边集数组
    public class EGGraph<T>:GraphAbstract<T>
    {
        #region 数据
        public Edge[] edges;
        public int edgeNum;
        #endregion
        
        #region 构造
        public EGGraph(GraphType type):base(type)
        {
            edges = new Edge[5];
        }
        #endregion
        
        #region 顶点
        public override T RemoveNode(int vertexIndex)
        {
            if(vertexIndex < 0 || vertexIndex >= vertexNum)
            {
                return default(T);
            }
            NodeAbstract<T> node = vertexs[vertexIndex];
            OneWayChainList<int> edgeIndexList = GetEdgeIndexArray(vertexIndex);
            foreach(int edgeIndex in edgeIndexList)
            {
                RemoveEdgeIndex(edgeIndex);
            }
            
            //最后调整顶点数组
            for(int i = vertexIndex;i < vertexNum - 1;i++)
            {
                vertexs[i] = vertexs[i + 1];
            }
            vertexs[vertexNum--] = null;
            return node.data;
        }
        
        private void RemoveEdgeIndex(int index)
        {
            for(int i = index;i < edgeNum - 1;i++)
            {
                edges[i] = edges[i + 1];
            }
        }
        

        #endregion
        
        #region 边
        public override void AddEdge(int start, int end, int len)
        {
            if(edgeNum >= edges.Length - 2)
            {
                Expand();
            }
            edges[edgeNum++] = new Edge(start,end,len);
            edges[edgeNum++] = new Edge(end,start,len);
        }

        public override void AddArc(int start, int end, int len)
        {
            if(edgeNum >= edges.Length - 1)
            {
                Expand();
            }
            edges[edgeNum++] = new Edge(start,end,len);
        }

        public override void RemvoeEdge(int start, int end)
        {
            OneWayChainList<int> edgeIndexList = GetEdgeIndexArray(start,end,true);
            foreach(int edgeIndex in edgeIndexList)
            {
                RemoveEdgeIndex(edgeIndex);
            }
        }

        public override void RemvoeArc(int start, int end)
        {
            OneWayChainList<int> edgeIndexList = GetEdgeIndexArray(start,end);
            foreach(int edgeIndex in edgeIndexList)
            {
                RemoveEdgeIndex(edgeIndex);
            }
        }
        #endregion
        
        #region 获取
        public override int GetEdgeWeight(int start, int end)
        {
            return 0;
        }
        
        private OneWayChainList<int> GetEdgeIndexArray(int index)
        {
            OneWayChainList<int> edgeIndex = new OneWayChainList<int>();
            Edge checkEdge;
            for(int i = 0;i < edgeNum;i++)
            {
                checkEdge = edges[i];
                if(checkEdge.start == index || checkEdge.end == index)
                {
                    edgeIndex.Append(i);
                }
            }

            return edgeIndex;
        }
        
        private OneWayChainList<int> GetEdgeIndexArray(int start,int end,bool findReverse = false)
        {
            OneWayChainList<int> edgeIndex = new OneWayChainList<int>();
            Edge checkEdge;
            for(int i = 0;i < edgeNum;i++)
            {
                checkEdge = edges[i];
                if((checkEdge.start == start && checkEdge.end == end) || (findReverse && checkEdge.start == end && checkEdge.end == start))
                {
                    edgeIndex.Append(i);
                    if(findReverse == false || (findReverse && edgeIndex.Count > 1))
                    {
                        break;
                    }
                }
            }
            return edgeIndex;
        }
        #endregion
        
        #region 扩容
        public override void Expand()
        {
            int newLength = vertexs.Length * 2;  
            NodeAbstract<T>[] newVertex = new NodeAbstract<T>[newLength];
            Edge[] newEdges = new Edge[newLength];
            for(int i = 0;i < vertexs.Length;i++)
            {
                newVertex[i] = vertexs[i];
                newEdges[i] = edges[i];
            }

            vertexs = newVertex;
            edges = newEdges;
        }
        #endregion
        
        #region 广度优先遍历

        public override ChainQueue<NodeAbstract<T>> BFSorder(int index)
        {
            return new ChainQueue<NodeAbstract<T>>();
        }
        #endregion
        
        #region 深度优先遍历

        public override ChainQueue<NodeAbstract<T>> DFSorder(int index)
        {
            return new ChainQueue<NodeAbstract<T>>();
        }
        #endregion
        
        #region 最小生成树

        public override Edge GetMinEdge()
        {
            return new Edge(-1,-1,0);
        }
        #endregion
        
        #region 获取节点

        public override NodeAbstract<T> GetNode(T data)
        {
            return new MatrixGraph.Node<T>(data);
        }

        #endregion
    }
}