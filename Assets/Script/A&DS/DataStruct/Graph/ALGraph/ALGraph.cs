using Common.DataStruct.Queue.ChainQueue;
using Common.DataStruct.Stack.ChianStack;
using DataStruct.Graph.MatrixGraph;
using DataStruct.Graph.Base;

namespace DataStruct.Graph.ALGraph
{
    //邻接表
    public class ALGraph<T>:GraphAbstract<T>
    {
        #region 构造
        public ALGraph(GraphType type):base(type){}
        #endregion
        
        #region 顶点
        public override T RemoveNode(int index)
        {
            RemoveEdgeIndex(index);
            T removeData = Vertexs[index].data;
            for(int i = index;i < vertexNum - 1;i++)
            {
                Vertexs[i] = Vertexs[i + 1];
            }

            vertexNum--;
            return removeData;
        }
        
        private void RemoveEdgeIndex(int index)
        {
            EdgeNode node;
            for(int i = 0;i < vertexNum;i++)
            {
                if(index == i)
                {
                    continue;
                }
                Vertexs[i].RemoveEdge(index);
            }
        }
        #endregion
        
        #region 扩容
        public override void Expand()
        {
            int newLength = vertexs.Length * 2;
            NodeAbstract<T>[] newVertex = new NodeAbstract<T>[newLength];
            for(int i = 0;i < vertexs.Length;i++)
            {
                newVertex[i] = vertexs[i];
            }

            vertexs = newVertex;
        }
        #endregion
        
        #region 边
        public override void AddEdge(int start,int end,int len)
        {
            NodeAbstract<T> node = GetNode(start);
            NodeAbstract<T> reverseNode = GetNode(end);
            if(node == null || reverseNode == null)
            {
                return;
            }
            node.AddEdge(end,len);
            reverseNode.AddEdge(start,len);
        }
        
        public override void AddArc(int start,int end,int len)
        {
            NodeAbstract<T> node = GetNode(start);
            if(node == null)
            {
                return;
            }
            node.AddEdge(end,len);
        }

        public override void RemvoeEdge(int start, int end)
        {
            VertexNode<T> startNode = GetNode(start) as VertexNode<T>;
            VertexNode<T> endNode = GetNode(end) as VertexNode<T>;
            if(startNode == null || endNode == null)
            {
                return;
            }
            startNode.RemoveEdge(end);
            endNode.RemoveEdge(start);
        }

        public override void RemvoeArc(int start, int end)
        {
            VertexNode<T> startNode = GetNode(start) as VertexNode<T>;
            if(startNode == null)
            {
                return;
            }
            startNode.RemoveEdge(end);
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
            ChainQueue<NodeAbstract<T>> checkQueue = new ChainQueue<NodeAbstract<T>>(),searchQueue = new ChainQueue<NodeAbstract<T>>();
            NodeAbstract<T> startNode = GetNode(index);
            startNode.isVisit = true;
            checkQueue.Enqueue(startNode);
            
            while(!checkQueue.IsEmpty())
            {
                VertexNode<T> node = checkQueue.Dequeue() as VertexNode<T>;
                NodeAbstract<T> visitNode;
                searchQueue.Enqueue(node);
                EdgeNode curNode = node.first_edge;
                while(curNode != null)
                {
                    visitNode = GetNode(curNode.vertexIndex);
                    if(visitNode != null && visitNode.isVisit == false)
                    {
                        checkQueue.Enqueue(visitNode);
                        visitNode.isVisit = true;
                    }
                    curNode = curNode.next;
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
            ChainStack<NodeAbstract<T>> checkQueue = new ChainStack<NodeAbstract<T>>();
            ChainQueue<NodeAbstract<T>> searchQueue = new ChainQueue<NodeAbstract<T>>();
            NodeAbstract<T> startNode = GetNode(index);
            startNode.isVisit = true;
            checkQueue.Push(startNode);
            
            while(!checkQueue.IsEmpty())
            {
                VertexNode<T> node = checkQueue.Pop() as VertexNode<T>;
                NodeAbstract<T> visitNode = null;
                searchQueue.Enqueue(node);
                EdgeNode curNode = node.first_edge;
                while(curNode != null)
                {
                    visitNode = GetNode(curNode.vertexIndex);
                    if(visitNode != null && visitNode.isVisit == false)
                    {
                        checkQueue.Push(visitNode);
                        visitNode.isVisit = true;
                    }
                    curNode = curNode.next;
                }
            }

            return searchQueue;
        }
        #endregion
        
        #region 最小生成树
        /// <summary>
        /// 获取最小边
        /// </summary>
        /// <returns></returns>
        public override Edge GetMinEdge()
        {
            int start = -1, end = -1, len = -1;
            VertexNode<T> node = null;
            NodeAbstract<T> checkNode = null;
            for(int i = 0;i < vertexNum;i++)
            {
                node = GetNode(i) as VertexNode<T>;
                if(node == null || node.isVisit == false)
                {
                    continue;
                }
                foreach(EdgeNode edge in node)
                {
                    checkNode = GetNode(edge.vertexIndex);
                    if(checkNode != null && checkNode.isVisit == false)
                    {
                        if(start < 0 || edge.len < len)
                        {
                            start = i;
                            end = edge.vertexIndex;
                            len = edge.len;
                        }
                    }
                }
            }
            vertexs[end].isVisit = true;
            return start > 0 ? new Edge(start, end, len) : null;
        }
        #endregion
        
        #region 获取数据
        public override NodeAbstract<T> GetNode(T data)
        {
            return new VertexNode<T>(data);
        }
        
        public VertexNode<T> GetNode(int index)
        {
            if(index < 0 || index >= vertexNum)
            {
                return null;
            }
            return Vertexs[index] as VertexNode<T>;
        }
        #endregion
    }
}