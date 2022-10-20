using System.Collections;
using System.Collections.Generic;
using DataStruct.Graph.Base;

namespace DataStruct.Graph.ALGraph
{
    public class VertexNode<T>:NodeAbstract<T>,IEnumerable
    {
        #region 数据
        public EdgeNode first_edge;
        public int count;
        //最早开始时间
        public int earliestStartTime;
        //最早结束时间
        public int lastestStartTime;
        //是否是关键路径
        public bool isCriticalPath;
        //入度数量
        public int inDegreeNum;
        #endregion
        
        #region 构造
        public VertexNode(T data):base(data)
        {
            count = 0;
        }
        #endregion
        
        #region 数据处理
        /// <summary>
        /// 添加边
        /// </summary>
        /// <param name="vertexIndex"></param>
        /// <param name="len"></param>
        public override void AddEdge(int vertexIndex,int len)
        {
            if (first_edge == null)
                first_edge = new EdgeNode(vertexIndex,len);
            else
            {
                EdgeNode node = first_edge;
                while(node.next != null)
                {
                    node = node.next;
                }
                node.SetNext(vertexIndex,len);
            }

            count++;
        }
        
        /// <summary>
        /// 删除边
        /// </summary>
        /// <param name="index"></param>
        public override  void RemoveEdge(int index)
        {
            EdgeNode parentNode = null,node = first_edge;            
            while(node != null)
            {
                if(node.vertexIndex == index)
                {
                    if (parentNode == null)
                        first_edge = null;
                    else
                        parentNode.next = node.next;
                    break;
                }

                parentNode = node;
                node = node.next;
            }

            count--;
        }
        
        /// <summary>
        /// 刷新关键路径状态
        /// </summary>
        public void RefreshCriticalPathState()
        {
            isCriticalPath = earliestStartTime - lastestStartTime == 0;
        }
        #endregion
        
        #region 迭代器
        public IEnumerator GetEnumerator()
        {
            EdgeNode current = first_edge;
            if(current != null)
            {
                yield return current;
            }
            while(current != null && current.next != null)
            {
                yield return current.next;
                current = current.next;
            }
        }
        #endregion
    }
}