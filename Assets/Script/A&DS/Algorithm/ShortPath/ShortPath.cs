using _3DMath;
using Common.DataStruct.Queue.ChainQueue;
using DataStruct.Graph.ALGraph;
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
                this.weight = weight <= 0 ? MathUtility.MaxValue : weight;
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
        public class FloydNode
        {
            #region 数据
            /// <summary>
            /// 路点
            /// </summary>
            public ChainQueue<int> pathNodes;
            /// <summary>
            /// 权重
            /// </summary>
            public int weight;
            #endregion
            
            #region 构造
            public FloydNode(int rowIndex,int colIndex,int weight)
            {
                pathNodes = new ChainQueue<int>().Enqueue(rowIndex).Enqueue(colIndex);
                if (rowIndex != colIndex && weight == 0)
                    this.weight = MathUtility.MaxValue;
                else
                    this.weight = weight;
            }
            #endregion
        }
        
        /// <summary>
        /// 弗洛伊德算法（基于邻接矩阵）
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static FloydNode[,] Floyd_ShortPath(MatrixGraph<T> graph)
        {
            int vertexNum = graph.vertexNum;
            FloydNode[,] floydNodes = TransformFloyNodeMatrix(graph.edgeMatrix,vertexNum);
            for(int centerIndex = 0;centerIndex < vertexNum;centerIndex++)
            {
                for(int enterIndex = 0;enterIndex < vertexNum;enterIndex++)
                {
                    if(enterIndex == centerIndex)
                    {
                        continue;
                    }
                    for(int outIndex = 0;outIndex < vertexNum;outIndex++)
                    {
                        if(outIndex == centerIndex || outIndex == enterIndex)
                        {
                            continue;
                        }
                        //围绕中心点周转的权重
                        int turnOverWeight = floydNodes[enterIndex, centerIndex].weight +
                                       floydNodes[centerIndex, outIndex].weight;
                        //无穷值不用记录
                        if(turnOverWeight >= MathUtility.MaxValue)
                        {
                            continue;
                        }
                        //周转权重小于原直达权重，则表示有最短路径
                        if(turnOverWeight < floydNodes[enterIndex,outIndex].weight)
                        {
                            //原路径或权重进行更新
                            floydNodes[enterIndex, outIndex].weight = turnOverWeight;
                            floydNodes[enterIndex,outIndex].pathNodes = new ChainQueue<int>(floydNodes[enterIndex,centerIndex].pathNodes).Enqueue(outIndex);
                        }
                    }
                }
            }

            return floydNodes;
        }
        
        public static FloydNode[,] TransformFloyNodeMatrix(int[,] edgeMatrix,int vertexNum)
        {
            if(edgeMatrix == null)
            {
                return null;
            }
            
            FloydNode[,] floydNodes = new FloydNode[vertexNum,vertexNum];
            for(int i = 0;i < vertexNum;i++)
            {
                for(int j = 0;j < vertexNum;j++)
                {
                    floydNodes[i,j] = new FloydNode(i,j,edgeMatrix[i,j]);
                }
            }

            return floydNodes;
        }
        #endregion
        
        #region A*搜索
        #region 邻接表搜索
        /// <summary>
        /// A*寻路_邻接表
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        // public static int[] AStarFindPath(ALGraph<T> graph,int startIndex,int endIndex)
        // {
        //     
        // }
        //
        // public static int
        #endregion
        
        #region 邻接矩阵搜索
        
        #endregion
        #endregion
    }
}