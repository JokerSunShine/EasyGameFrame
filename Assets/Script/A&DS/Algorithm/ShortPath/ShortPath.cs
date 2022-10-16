using Common.DataStruct.Queue.ChainQueue;
using DataStruct.Graph.MatrixGraph;

namespace Algorithm.ShortPath
{
    public class ShortPath<T>
    {
        #region 迪杰斯特拉算法
        public class DJSNode<T>
        {
            #region 数据
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
            public DJSNode(bool state,int weight)
            {
                pathNodes = new ChainQueue<int>();
                this.state = state;
                this.weight = weight;
            }
            #endregion
        }
        
        /// <summary>
        /// 迪杰斯特拉算法
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static DJSNode<T>[] Dijkstra_ShortPath(MatrixGraph<T> graph,int startIndex)
        {
            if(startIndex < 0 || startIndex >= graph.vertexNum)
            {
                return null;
            }
            //寻路点数组
            DJSNode<T>[] pathNodeArray = new DJSNode<T>[graph.vertexNum];
            for(int i = 0;i < graph.vertexNum;i++)
            {
                pathNodeArray[i] = new DJSNode<T>(false,graph.edgeMatrix[startIndex,i]);
            }

            pathNodeArray[startIndex].weight = 0;
            pathNodeArray[startIndex].state = true;
            int searchNode = startIndex;
            
            for(int i = 0;i < graph.vertexNum;i++)
            {
                DJSNode<T> minDjsNode = null;
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
                        if(pathNodeArray[j].state == false && minDjsNode.weight + graph.edgeMatrix[searchNode,j] < pathNodeArray[j].weight)
                        {
                            pathNodeArray[j].weight = minDjsNode.weight + graph.edgeMatrix[searchNode, j];
                            // pathNodeArray[j].pathNodes =  ;
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