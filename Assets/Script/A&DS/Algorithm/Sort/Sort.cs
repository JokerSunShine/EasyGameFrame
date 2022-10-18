using Common.DataStruct.Queue.ChainQueue;
using Common.OneWayChainList;
using DataStruct.Graph.MatrixGraph;

namespace Algorithm.Sort
{
    public class Sort<T>
    {
        #region 拓补排序
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
        #endregion
    }
}