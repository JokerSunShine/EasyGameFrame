using System;
using System.Collections;
using System.Collections.Generic;
using DataStruct.Graph.Base;
using DataStruct.Graph.EGGraph;
using DataStruct.Graph.MatrixGraph;

namespace Algorithm.MiniSpanTree.MiniSpanTree
{
    public class MiniSpanTree<T>
    {
        #region 数据
        private const int MAXINT = 214783647;
        #endregion
        
        #region 普利姆算法
         /// <summary>
        /// 普利姆算法
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static Edge[] Prim(MatrixGraph<T> graph)
        {
            graph.ClearNodeVisit();
            int treeCount = graph.vertexNum - 1;
            Edge[] edges = new Edge[treeCount];
            //weights数组用于记录B类顶点到A类顶点的权重
            //points数组用于记录最小生成树中各个顶点之间的关联
            int[] weights = new int[graph.vertexNum],points = new int[graph.vertexNum];
            for(int i = 0;i < graph.vertexNum;i++)
            {
                weights[i] = MAXINT;
                points[i] = -1;
            }

            //选择数组中的第一个顶点，开始寻找最小生成树
            weights[0] = 0;
            
            //遍历最小生成树路径
            for(int i = 0;i < treeCount;i++)
            {
                //从权重列表中找到未被访问过权值最小的顶点位置（划分到A类的顶点下标）
                int u = GetMinKey(weights, graph.Vertexs);
                //顶点划分到A类
                graph.Vertexs[u].isVisit = true;

                //由于新顶点加入A类，因此需要更新权重数组中的数据
                for(int v = 0;v < graph.vertexNum;v++)
                {
                    //如果类B中存在到下标u顶点的权值比weight数组中记录值还小，表明新顶点的加入，使得B到类A顶点的权重有了更好的选择
                    if(graph.edgeMatrix[u,v] != 0 && graph.Vertexs[v].isVisit == false && graph.edgeMatrix[u,v] < weights[v])
                    {
                        points[v] = u;
                        weights[v] = graph.edgeMatrix[u, v];
                    }
                }
            }

            for (int i = 1; i < graph.vertexNum;i++)
            {
                edges[i - 1] = new Edge(i,points[i],weights[i]);
            }

            return edges;
        }
        
        /// <summary>
        /// 寻找最小权重顶点下标
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="vertexs"></param>
        /// <returns></returns>
        private static int GetMinKey(int[] weights,NodeAbstract<T>[] vertexs)
        {
            int min = MAXINT, minIndex = 0;
            for(int i = 0;i < weights.Length;i++)
            {
                if(vertexs[i].isVisit == false && weights[i] < min)
                {
                    min = weights[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }
        #endregion
       
        #region 克鲁斯卡尔算法
        /// <summary>
        /// 克鲁斯卡尔算法
        /// </summary>
        /// <param name="graph"></param>
        public static Edge[] Kruskal(EGGraph<T> graph)
        {
            //顶点标记
            int[] signs = new int[graph.vertexNum];
            for(int i = 0;i < graph.vertexNum;i++)
            {
                signs[i] = i;
            }
            
            Edge[] minTree = new Edge[graph.vertexNum - 1];
            IComparer<Edge> edgeCompare = new EdgeCompare();
            Array.Sort(graph.edges,edgeCompare);

            int treeIndex = 0, start, end;
            for(int i = 0;i < graph.edgeNum;i++)
            {
                start = graph.edges[i].start;
                end = graph.edges[i].end;
                //如果顶点位置存在切顶点的标记不同，说明不在一个集合内，不会产生回路
                if(signs[start] != signs[end])
                {
                    //记录边，作为最小生成树的组成部分
                    minTree[treeIndex++] = graph.edges[i];
                    //将新加入生成树的顶点标记券更改为一样的
                    int endSign = signs[end];
                    for(int k = 0;k < graph.vertexNum;k++)
                    {
                        if(signs[k] == endSign)
                        {
                            signs[k] = signs[start];
                        }
                    }
                }
                
                //如果选择的边数量和顶点相差1，证明最小生成树以生成，退出循环
                if(treeIndex >= graph.vertexNum - 1)
                {
                    break;
                }
            }

            return minTree;
        }

        public class EdgeCompare:IComparer<Edge>
        {
            public int Compare(Edge x,Edge y)
            {
                if(x == null)
                {
                    return 1;
                }
                else if(y == null)
                {
                    return -1;
                }
                return x.len - y.len;
            }
        }
        #endregion
    }
}