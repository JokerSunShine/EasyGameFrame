using Common.DataStruct.Queue.ChainQueue;
using DataStruct.Graph.MatrixGraph;

namespace Algorithm.ShortPath
{
    public class ShortPath<T>
    {
        #region 迪杰斯特拉算法
        public class DJSNode
        {
            #region 数据
            /// <summary>
            /// 节点下标
            /// </summary>
            public int nodeIndex;
            /// <summary>
            /// 寻路点
            /// </summary>
            public ChainQueue<int> pathNodes;
            /// <summary>
            /// 标记状态
            /// </summary>
            public bool state;
            /// <summary>
            /// 权重
            /// </summary>
            public int weight;
            #endregion
            
            #region 构造
            public DJSNode(bool state,int weight,int startIndex,int nodeIndex)
            {
                pathNodes = new ChainQueue<int>();
                this.state = state;
                this.weight = weight <= 0 ? 214783647 : weight;
                this.nodeIndex = nodeIndex;
                if(weight > 0)
                {
                    pathNodes.Enqueue(startIndex).Enqueue(nodeIndex);
                }
            }
            #endregion
        }
        
        /// <summary>
        /// 迪杰斯特拉算法(有权图，无权无效)
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static DJSNode[] Dijkstra_ShortPath(MatrixGraph<T> graph,int startIndex)
        {
            if(startIndex < 0 || startIndex >= graph.vertexNum)
            {
                return null;
            }
            //寻路点数组
            DJSNode[] pathNodeArray = new DJSNode[graph.vertexNum];
            for(int i = 0;i < graph.vertexNum;i++)
            {
                pathNodeArray[i] = new DJSNode(false,graph.edgeMatrix[startIndex,i],startIndex,i);
            }

            pathNodeArray[startIndex].weight = 0;
            pathNodeArray[startIndex].state = true;
            int searchNode = startIndex;
            
            for(int i = 0;i < graph.vertexNum;i++)
            {
                DJSNode minDjsNode = null;
                //选择到各顶点权值最小的顶点，即为本次能确定最短路径的顶点
                for(int k = 0;k < graph.vertexNum;k++)
                {
                    if(pathNodeArray[k].state == false && (minDjsNode == null || pathNodeArray[k].weight < minDjsNode.weight))
                    {
                        minDjsNode = pathNodeArray[k];
                        searchNode = k;
                    }
                }
                
                if(minDjsNode != null)
                {
                    minDjsNode.state = true;    
                    for(int j = 0;j < graph.vertexNum;j++)
                    {
                        if(pathNodeArray[j].state == false && graph.edgeMatrix[searchNode,j] > 0 && minDjsNode.weight + graph.edgeMatrix[searchNode,j] < pathNodeArray[j].weight)
                        {
                            pathNodeArray[j].weight = minDjsNode.weight + graph.edgeMatrix[searchNode, j];
                            pathNodeArray[j].pathNodes = new ChainQueue<int>(minDjsNode.pathNodes).Enqueue(j);
                        }
                    }
                }
            }

            return pathNodeArray;
        }
        #endregion
        
        #region 弗洛伊德算法
        #endregion
    }
}