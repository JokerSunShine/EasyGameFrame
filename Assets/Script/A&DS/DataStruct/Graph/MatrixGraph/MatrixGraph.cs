using Common.DataStruct.Queue.ChainQueue;
using Common.DataStruct.Stack.ChianStack;
using DataStruct.Graph.Base;


namespace DataStruct.Graph.MatrixGraph
{
    //邻接矩阵
    public class MatrixGraph<T>:GraphAbstract<T>
    {
        #region 数据
        //边矩阵（顶点对应的弧和弧的权重）
        public int[,] edgeMatrix;
        #endregion
        
        #region 构造
        public MatrixGraph(GraphType type):base(type)
        {
            edgeMatrix = new int[5,5];
        }
        #endregion
        
        #region 顶点
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override T RemoveNode(int index)
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
            NodeAbstract<T> node = vertexs[index];
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
                vertexs[i] = vertexs[i + 1];
            }

            vertexs[vertexNum--] = null;
            return node.data;
        }
        #endregion
        
        #region 边
        /// <summary>
        /// 无向图添加边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="len"></param>
        public override void AddEdge(int start,int end,int len)
        {
            edgeMatrix[start,end] = len;
            edgeMatrix[end,start] = len;
            edgeNum++;
        }
        
        /// <summary>
        /// 有向图添加弧
        /// </summary>
        public override void AddArc(int start,int end,int len)
        {
            edgeMatrix[start, end] = len;
            edgeNum++;
        }
        
        /// <summary>
        /// 无向图删除边
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public override void RemvoeEdge(int start,int end)
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
        public override void RemvoeArc(int start,int end)
        {
            edgeMatrix[start, end] = 0;
            edgeNum--;            
        }
        #endregion
        
        #region 获取
        /// <summary>
        /// 获取边权重
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override int GetEdgeWeight(int x,int y)
        {
            if(x >= vertexNum || y >= vertexNum)
            {
                return -1;
            }

            return edgeMatrix[x, y];
        }
        #endregion
        
        #region 扩容
        public override void Expand()
        {
            int newLength = vertexs.Length * 2;
            NodeAbstract<T>[] newVertex = new NodeAbstract<T>[newLength];
            int[,] newEdgeMatrix = new int[newLength, newLength];
            for(int i = 0;i < vertexs.Length;i++)
            {
                newVertex[i] = vertexs[i];
                for(int j = 0;j < vertexs.Length;j++)
                {
                    newEdgeMatrix[i, j] = edgeMatrix[i, j];
                }
            }

            vertexs = newVertex;
            edgeMatrix = newEdgeMatrix;
        }
        #endregion
        
        #region 广度优先遍历
        /// <summary>
        /// 广度优先遍历
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override ChainQueue<NodeAbstract<T>> BFSorder(int index)
        {
            ChainQueue<NodeAbstract<T>> checkQueue = new ChainQueue<NodeAbstract<T>>();
            ChainQueue<NodeAbstract<T>> searchQueue = new ChainQueue<NodeAbstract<T>>();
            NodeAbstract<T> startNode = vertexs[index];
            startNode.isVisit = true;
            checkQueue.Enqueue(startNode);
            
            while(!checkQueue.IsEmpty())
            {
                NodeAbstract<T> node = checkQueue.Dequeue();
                searchQueue.Enqueue(node);
                int curIndex = GetIndex(node.data);
                NodeAbstract<T> checkVertex = null;
                if(curIndex >= 0)
                {
                    for(int i = 0;i < vertexNum;i++)
                    {
                        //收集检测节点指向的下一个未被访问过的节点
                        if(edgeMatrix[curIndex,i] > 0 && vertexs[i].isVisit == false)
                        {
                            checkVertex = vertexs[i];
                            checkQueue.Enqueue(checkVertex);
                            checkVertex.isVisit = true;
                        }
                    }
                }
            }
            return searchQueue;
        }
        #endregion
        
        #region 深度优先遍历
        /// <summary>
        /// 深度优先遍历
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override ChainQueue<NodeAbstract<T>> DFSorder(int index)
        {
            ChainStack<NodeAbstract<T>> checkStack = new ChainStack<NodeAbstract<T>>();
            ChainQueue<NodeAbstract<T>> searchQueue = new ChainQueue<NodeAbstract<T>>();
            NodeAbstract<T> startNode = vertexs[index];
            startNode.isVisit = true;
            checkStack.Push(startNode);
            
            while(!checkStack.IsEmpty())
            {
                NodeAbstract<T> node = checkStack.Pop();
                searchQueue.Enqueue(node);
                int curIndex = GetIndex(node.data);
                NodeAbstract<T> checkVertex = null;
                if(curIndex >= 0)
                {
                    for(int i = 0;i < vertexNum;i++)
                    {
                        //收集检测节点指向的下一个未被访问过的节点
                        if(edgeMatrix[curIndex,i] > 0 && vertexs[i].isVisit == false)
                        {
                            checkVertex = vertexs[i];
                            checkStack.Push(checkVertex);
                            checkVertex.isVisit = true;
                        }
                    }
                }
            }
            return searchQueue;
        }
        #endregion
        
        #region 最小生成树
        public override Edge GetMinEdge()
        {
            int start = -1, end = -1, len = -1;
            for(int i = 0;i < vertexNum;i++)
            {
                for(int j = 0;j < vertexNum;j++)
                {
                    //遍历矩阵寻找未被访问过的最小边
                    if(vertexs[i].isVisit && !vertexs[j].isVisit && edgeMatrix[i,j] != 0)
                    {
                        if(start < 0 || edgeMatrix[i,j] < len)
                        {
                            start = i;
                            end = j;
                            len = edgeMatrix[i, j];
                        }
                    }
                }
            }
            vertexs[end].isVisit = true;
            return start >= 0 ? new Edge(start, end, len) : null;
        }
        #endregion

        #region 获取节点
        public override NodeAbstract<T> GetNode(T data)
        {
            return new Node<T>(data);
        }
        #endregion
    }
}