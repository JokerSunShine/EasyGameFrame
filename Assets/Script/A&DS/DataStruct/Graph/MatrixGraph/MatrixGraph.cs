using Common.DataStruct.Queue.ChainQueue;
using Common.DataStruct.Stack.ChianStack;

namespace DataStruct.Graph.MatrixGraph
{
    public enum MatrixGraphType
    {
        UndirectedGraph,
        DirectedGraph,        
    }
    //邻接矩阵
    public class MatrixGraph<T>
    {
        #region 数据
        //节点列表
        private Node<T>[] Vertex;
        //边矩阵（顶点对应的弧和弧的权重）
        private int[,] edgeMatrix;
        //顶点数和弧数
        private int vertexNum, edgeNum;
        private MatrixGraphType type;
        #endregion
        
        #region 构造
        public MatrixGraph(MatrixGraphType type)
        {
            Vertex = new Node<T>[5];
            edgeMatrix = new int[5,5];
            vertexNum = edgeNum = 0;
            this.type = type;
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
            if(type == MatrixGraphType.DirectedGraph)
                AddArc(start,end,len);
            else
                AddEdge(start,end,len);
        }
        
        /// <summary>
        /// 无向图添加边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="len"></param>
        private void AddEdge(int start,int end,int len)
        {
            edgeMatrix[start,end] = len;
            edgeMatrix[end,start] = len;
            edgeNum++;
        }
        
        /// <summary>
        /// 有向图添加弧
        /// </summary>
        private void AddArc(int start,int end,int len)
        {
            edgeMatrix[start, end] = len;
            edgeNum++;
        }
        
        /// <summary>
        /// 图删除边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void GraphRemoveEdge(int start,int end)
        {
            if(type == MatrixGraphType.DirectedGraph)
                RemvoeArc(start,end);
            else
                RemvoeEdge(start,end);           
        }
        
        /// <summary>
        /// 无向图删除边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void RemvoeEdge(int start,int end)
        {
            edgeMatrix[start, end] = 0;
            edgeMatrix[end, start] = 0;
            edgeNum--;
        }
        
        /// <summary>
        /// 有向图删除弧
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void RemvoeArc(int start,int end)
        {
            edgeMatrix[start, end] = 0;
            edgeNum--;            
        }
        #endregion
        
        #region 顶点
        /// <summary>
        /// 添加顶点
        /// </summary>
        /// <param name="data"></param>
        public void AddNode(T data)
        {
            if(vertexNum == Vertex.Length)
                Expand();
            Vertex[vertexNum++] = new Node<T>(data);
        }
        
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public T RemoveNode(T data)
        {
            return RemoveNode(GetIndex(data));
        }
        
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T RemoveNode(int index)
        {
            if(index < 0 || index >= vertexNum)
            {
                return default(T);
            }

            //以删除节点构成的十字区间进行分割
            //十字中心点
            //右上角：向左缩进
            //左下角：向上缩进
            //右下角：向左并向上缩进
            Node<T> node = Vertex[index];
            for(int i = 0;i < vertexNum;i++)
            {
                for(int j = 0;j < vertexNum;j++)
                {
                    //右下角
                    if(i > index && j > index)
                    {
                        edgeMatrix[i - 1, j - 1] = edgeMatrix[i, j];
                    }
                    else if(i > index)
                    {
                        //右上角
                        edgeMatrix[i - 1, j] = edgeMatrix[i, j];
                    }
                    else if(j > index)
                    {
                        //左下角
                        edgeMatrix[i, j - 1] = edgeMatrix[i, j];
                    }
                }
            }
            
            //缩进后最后的节点有冗余数据，删除处理
            for(int i = 0;i < vertexNum;i++)
            {
                edgeMatrix[vertexNum - 1,i] = 0;
                edgeMatrix[i, vertexNum - 1] = 0;
            }
            
            //最后调整顶点数组
            for(int i = index;i < vertexNum - 1;i++)
            {
                Vertex[i] = Vertex[i + 1];
            }

            Vertex[vertexNum--] = null;
            return node.data;
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
                if(data.Equals(Vertex[i].data))
                {
                    return i;
                }   
            }
            
            return -1;
        }
        
        /// <summary>
        /// 获取边权重
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetEdgeWeight(int x,int y)
        {
            if(x >= vertexNum || y >= vertexNum)
            {
                return -1;
            }

            return edgeMatrix[x, y];
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
                Vertex[i].ResetState();
            }
        }
        #endregion
        
        #region 扩容
        private void Expand()
        {
            int newLength = Vertex.Length * 2;
            Node<T>[] newVertex = new Node<T>[newLength];
            int[,] newEdgeMatrix = new int[newLength, newLength];
            for(int i = 0;i < Vertex.Length;i++)
            {
                newVertex[i] = Vertex[i];
                for(int j = 0;j < Vertex.Length;j++)
                {
                    newEdgeMatrix[i, j] = edgeMatrix[i, j];
                }
            }

            Vertex = newVertex;
            edgeMatrix = newEdgeMatrix;
        }
        #endregion
        
        #region 广度优先遍历
        /// <summary>
        /// 广度优先遍历（包含连通分量）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ChainQueue<Node<T>> GraphBFS(int index)
        {
            ClearNodeVisit();
            ChainQueue<Node<T>> queue = BFSorder(index);
            for(int i = 0;i < vertexNum;i++)
            {
                //连通分量构建树
                if(Vertex[i].IsVisited == false)
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
        public ChainQueue<Node<T>> SingleBFS(int index)
        {
            ClearNodeVisit();
            return BFSorder(index);
        }
        
        /// <summary>
        /// 广度优先遍历
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private ChainQueue<Node<T>> BFSorder(int index)
        {
            ChainQueue<Node<T>> checkQueue = new ChainQueue<Node<T>>();
            ChainQueue<Node<T>> searchQueue = new ChainQueue<Node<T>>();
            Node<T> startNode = Vertex[index];
            startNode.IsVisited = true;
            checkQueue.Enqueue(startNode);
            
            while(!checkQueue.IsEmpty())
            {
                Node<T> node = checkQueue.Dequeue();
                searchQueue.Enqueue(node);
                int curIndex = GetIndex(node.data);
                Node<T> checkVertex = null;
                if(curIndex >= 0)
                {
                    for(int i = 0;i < vertexNum;i++)
                    {
                        //收集检测节点指向的下一个未被访问过的节点
                        if(edgeMatrix[curIndex,i] > 0 && Vertex[i].IsVisited == false)
                        {
                            checkVertex = Vertex[i];
                            checkQueue.Enqueue(checkVertex);
                            checkVertex.IsVisited = true;
                        }
                    }
                }
            }
            return searchQueue;
        }
        #endregion
        
        #region 深度优先遍历
        /// <summary>
        /// 深度优先遍历（包含连通分量）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ChainQueue<Node<T>> GraphDFS(int index)
        {
            ClearNodeVisit();
            ChainQueue<Node<T>> queue = DFSorder(index);
            for(int i = 0;i < vertexNum;i++)
            {
                //连通分量构建树
                if(Vertex[i].IsVisited == false)
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
        public ChainQueue<Node<T>> SingleDFS(int index)
        {
            ClearNodeVisit();
            return DFSorder(index);
        }
        
        /// <summary>
        /// 深度优先遍历
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private ChainQueue<Node<T>> DFSorder(int index)
        {
            ChainStack<Node<T>> checkStack = new ChainStack<Node<T>>();
            ChainQueue<Node<T>> searchQueue = new ChainQueue<Node<T>>();
            Node<T> startNode = Vertex[index];
            startNode.IsVisited = true;
            checkStack.Push(startNode);
            
            while(!checkStack.IsEmpty())
            {
                Node<T> node = checkStack.Pop();
                searchQueue.Enqueue(node);
                int curIndex = GetIndex(node.data);
                Node<T> checkVertex = null;
                if(curIndex >= 0)
                {
                    for(int i = 0;i < vertexNum;i++)
                    {
                        //收集检测节点指向的下一个未被访问过的节点
                        if(edgeMatrix[curIndex,i] > 0 && Vertex[i].IsVisited == false)
                        {
                            checkVertex = Vertex[i];
                            checkStack.Push(checkVertex);
                            checkVertex.IsVisited = true;
                        }
                    }
                }
            }
            return searchQueue;
        }
        #endregion
        
        #region 最小生成树
        /// <summary>
        /// 获取最小数边列表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Edge[] GetEdges(int index)
        {
            ClearNodeVisit();
            //遍历的树的边 = 顶点总数 - 1
            Edge[] edges = new Edge[vertexNum - 1];
            Vertex[index].IsVisited = true;
            for(int i = 0;i < edges.Length;i++)
            {
                edges[i] = GetMinEdge();
            }

            return edges;
        }
        
        private Edge GetMinEdge()
        {
            Edge min = null;
            for(int i = 0;i < vertexNum;i++)
            {
                for(int j = 0;j < vertexNum;j++)
                {
                    //遍历矩阵寻找未被访问过的最小边
                    if(Vertex[i].IsVisited && !Vertex[j].IsVisited && edgeMatrix[i,j] != 0)
                    {
                        if(min == null)
                        {
                            min = new Edge(i,j,edgeMatrix[i,j]);
                        }
                        else
                        {
                            if(edgeMatrix[i,j] < min.len)
                            {
                                min = new Edge(i,j,edgeMatrix[i,j]);
                            }
                        }
                    }
                }
            }

            if (min != null)
                Vertex[min.end].IsVisited = true;
            return min;
        }
        #endregion
    }
}