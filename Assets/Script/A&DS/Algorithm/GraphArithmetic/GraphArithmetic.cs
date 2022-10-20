using Common.DataStruct.Queue.ChainQueue;
using Common.DataStruct.Stack.ChianStack;
using Common.OneWayChainList;
using DataStruct.Graph.ALGraph;
using DataStruct.Graph.MatrixGraph;

namespace Algorithm.GraphArithmetic
{
    /// <summary>
    /// 图算法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphArithmetic<T>
    {
        #region 拓补排序（通过图寻找一个可执行顺序列表）
        public static OneWayChainList<int> TopologicalSort(MatrixGraph<T> graph)
        {
            //拓补结果
            OneWayChainList<int> topoRes = new OneWayChainList<int>();
            //检测队列
            ChainQueue<int> checkQueue = new ChainQueue<int>();
            //度统计值
            int[] inDegree = new int[graph.vertexNum];
            for(int i = 0;i < graph.vertexNum;i++)
            {
                for(int j = 0;j < graph.vertexNum;j++)
                {
                    //入度统计
                    if(graph.edgeMatrix[j,i] > 0)
                    {
                        inDegree[i]++;
                    }
                }
            }
            
            //取入度为0的节点
            for(int i = 0;i < inDegree.Length;i++)
            {
                if (inDegree[i] == 0)
                    checkQueue.Enqueue(i);
            }
            
            //遍历并取出度数为0的顶点，队列方式取值类似广度优先遍历
            while(!checkQueue.IsEmpty())
            {
                int vertexIndex = checkQueue.Dequeue();
                topoRes.Append(vertexIndex);
                for(int i = 0;i < graph.vertexNum;i++)
                {
                    if(graph.edgeMatrix[vertexIndex,i] > 0)
                    {
                        inDegree[i]--;
                        if(inDegree[i] == 0)
                        {
                            checkQueue.Enqueue(i);
                        }
                    }
                }
            }

            return topoRes;
        }
        
        public static OneWayChainList<int> TopologicalSort(ALGraph<T> graph)
        {
            //拓补结果
            OneWayChainList<int> topoRes = new OneWayChainList<int>();
            //检测队列
            ChainQueue<int> checkQueue = new ChainQueue<int>();
            //度统计值
            int[] inDegree = FindInDegree(graph);
            
            //取入度为0的节点
            for(int i = 0;i < inDegree.Length;i++)
            {
                if (inDegree[i] == 0)
                    checkQueue.Enqueue(i);
            }
            
            //遍历并取出度数为0的顶点，队列方式取值类似广度优先遍历
            while(!checkQueue.IsEmpty())
            {
                int vertexIndex = checkQueue.Dequeue();
                topoRes.Append(vertexIndex);
                VertexNode<T> node = graph.GetNode(vertexIndex);
                if(node != null && node.first_edge != null)
                {
                    EdgeNode edge = node.first_edge;
                    //出度数据移除
                    while(edge != null)
                    {
                        inDegree[edge.vertexIndex]--;
                        if(inDegree[edge.vertexIndex] == 0)
                        {
                            checkQueue.Enqueue(edge.vertexIndex);
                        }
                        edge = edge.next;
                    }
                }
            }

            return topoRes;
        }
        
        /// <summary>
        /// 邻接表入度统计
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        private static int[] FindInDegree(ALGraph<T> graph)
        {
            int[] inDegree = new int[graph.vertexNum];
            for(int i = 0;i < graph.vertexNum;i++)
            {
                VertexNode<T> node = graph.GetNode(i);
                if(node == null)
                {
                    continue;
                }
                EdgeNode edge = node.first_edge;
                while(edge != null)
                {
                    inDegree[edge.vertexIndex]++;
                    edge = edge.next;
                }
            }

            return inDegree;
        }
        #endregion
        
        #region 关键路径(提供每个事件节点的最早开始时间和最晚开始时间)
        /// <summary>
        /// 关键路径
        /// </summary>
        public static void CriticalPath(ALGraph<T> graph)
        {
            OneWayChainList<int> list = TopologicalSort(graph);
            if (list.Count < graph.vertexNum)
            {
                //说明有回环，无法找到关键路径
                return;
            }

            CalcETE(graph,list);
            CalcLTE(graph,list);
            RefreshCriticalPathState(graph);
        }
        
        /// <summary>
        /// 计算最早开始时间
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="toposortResult"></param>
        private static void CalcETE(ALGraph<T> graph,OneWayChainList<int> toposortResult)
        {
            VertexNode<T> node,outDegreeNode;
            EdgeNode edge;
            foreach(int vertexIndex in toposortResult)
            {
                node = graph.GetNode(vertexIndex);
                if(node == null || node.first_edge == null)
                {
                    continue;
                }

                edge = node.first_edge;
                while(edge != null)
                {
                    //如果起点节点的开始时间 + 出度权重 > 出度节点的开始时间，说明需要更新最早开始时间
                    outDegreeNode = graph.GetNode(edge.vertexIndex);
                    if(outDegreeNode != null && node.earliestStartTime + edge.len > outDegreeNode.earliestStartTime)
                    {
                        outDegreeNode.earliestStartTime = node.earliestStartTime + edge.len;
                    }
                    edge = edge.next;
                }
            }
        }
        
        /// <summary>
        /// 计算最晚开始时间
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="toposortResult"></param>
        private static void CalcLTE(ALGraph<T> graph,OneWayChainList<int> toposortResult)
        {
            ChainStack<int> stack = new ChainStack<int>(toposortResult);
            VertexNode<T> node,outDegreeNode,maxNode = graph.GetNode(graph.vertexNum - 1);
            EdgeNode edge;
            //初始化原最晚开始时间 = 最早开始时间
            for(int i = 0;i < graph.vertexNum;i++)
            {
                node = graph.GetNode(i);
                if(node != null)
                {
                    node.lastestStartTime = maxNode.earliestStartTime;
                }
            }
            
            while(!stack.IsEmpty())
            {
                int vertexIndex = stack.Pop();
                node = graph.GetNode(vertexIndex);
                if(node == null || node.first_edge == null)
                {
                    continue;
                }
                
                edge = node.first_edge;
                while(edge != null)
                {
                    //如果出度节点的最晚开始时间 - 出度权重 < 起点节点的最晚开始时间，说明需要更新最晚开始时间
                    outDegreeNode = graph.GetNode(edge.vertexIndex);
                    if(outDegreeNode != null && outDegreeNode.lastestStartTime - edge.len < node.lastestStartTime)
                    {
                        node.lastestStartTime = outDegreeNode.lastestStartTime - edge.len;
                    }
                    edge = edge.next;
                }
            }
        }
        
        /// <summary>
        /// 刷新关键路径状态
        /// </summary>
        private static void RefreshCriticalPathState(ALGraph<T> graph)
        {
            for(int i = 0;i < graph.vertexNum;i++)
            {
                VertexNode<T> node = graph.GetNode(i);
                if(node == null)
                {
                    continue;
                }
                node.RefreshCriticalPathState();
            }
        }
        #endregion
    }
}