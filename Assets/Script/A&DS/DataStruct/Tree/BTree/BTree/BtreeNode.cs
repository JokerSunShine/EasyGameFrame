using System;
using Common.OneWayChainList;
using UnityEngine;

namespace DataStruct.Tree.BTree.BTree
{
    public class BtreeNode<T>
    {
        #region 数据
        private T[] values;
        //关键字列表
        public T[] Values
        {
            get => values;
        }
        //关键字数量
        private int count;
        
        //阶
        private int order = 0;
        
        private int minCount = -1;
        //最小关键值数量
        public int MinCount
        {
            get
            {
                if(minCount < 0)
                {
                    minCount = Mathf.CeilToInt(order * 0.5f) - 1;
                }

                return minCount;
            }
        }

        private int maxCount = -1;
        //最大关键值数量
        public int MaxCount
        {
            get
            {
                if(maxCount < 0)
                {
                    maxCount = order;
                }

                return maxCount;
            }
        }
        //子节点列表
        public BtreeNode<T>[] childs;
        //是否是子节点
        public bool isLeaf;
        //对比方法
        public Func<T, T, int> compareFunc;
        #endregion
        
        #region 构造
        public BtreeNode(int order,bool isLeaf,Func<T,T,int> compareFunc,int count = 0)
        {
            this.order = order;
            this.isLeaf = isLeaf;
            this.compareFunc = compareFunc;
            this.count = count;
            values = new T[MaxCount];
            childs = new BtreeNode<T>[MaxCount + 1];
        }
        #endregion
        
        #region 功能
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public BtreeNode<T> FindNode(T data)
        {
            if(compareFunc == null)
            {
                return null;
            }
            
            int i = 0;
            while(i < count && compareFunc(data,Values[i]) > 0)
            {
                i++;
            }
            
            if(Values[i].Equals(data))
            {
                return this;
            }
            
            if(isLeaf)
            {
                return null;
            }

            return childs[i].FindNode(data);
        }

        /// <summary>
        /// 遍历（中序遍历，遍历结果从小到大）
        /// </summary>
        public void Traversal(ref OneWayChainList<T> traversaList,Action<T> action = null)
        {
            if(Values == null || count <= 0)
            {
                return;
            }
            
            if(isLeaf == false && childs != null && childs.Length > 0)
            {
                for(int i = 0;i < childs.Length;i++)
                {
                    childs[i].Traversal(ref traversaList,action);
                }
            }
            
            foreach(var data in Values)
            {
                if(traversaList != null)
                {
                    traversaList.Append(data);
                }
                
                if(action != null)
                {
                    action(data);
                }
            }
        }
        #endregion
        
        #region 加入数据
        /// <summary>
        /// 未满数据插入
        /// </summary>
        /// <param name="data">插入的数据</param>
        public void BTreeInserNodeNotFull(T data)
        {
            //初始化节点的最后一个关键字的位置
            int i = count - 1;
            
            if(isLeaf)
            {
                //升序插入新数据
                while(i >= 0 && compareFunc(Values[i],data) > 0)
                {
                    Values[i + 1] = Values[i];
                    i--;
                }

                Values[i + 1] = data;
                count++;
            }
            else
            {
                while(i >= 0 && compareFunc(Values[i],data) > 0)
                {
                    i--;
                }

                int nextNodeIndex = i + 1;
                //即将插入的点如果是满的话，就进行分割处理
                if(childs[nextNodeIndex].IsMaxCount())
                {
                    SplitChild(nextNodeIndex);
                }
                childs[nextNodeIndex].BTreeInserNodeNotFull(data);
            }
        }
        #endregion
        
        #region 节点分割
        /// <summary>
        /// 当前节点分割指定的子节点
        /// </summary>
        /// <param name="i">分割的子节点下标</param>
        public void SplitChild(int i)
        {
            if(childs == null || i >= childs.Length)
            {
                return;
            }

            BtreeNode<T> childNode = childs[i];
            if(childNode.IsMaxCount() == false)
            {
                return;
            }
            //分割的数据长度
            int splitLength = (int)((order + 1) * 0.5f) - 1;
            //构建一个新的分割节点
            BtreeNode<T> newSplitTreeNode = new BtreeNode<T>(childNode.order,childNode.isLeaf,childNode.compareFunc,splitLength);
            //将分割内容分给新的节点
            for(int j = 0;j < splitLength;j++)
            {
                newSplitTreeNode.Values[j] = childNode.Values[j + childNode.MinCount + 1];
            }
            //如果分割的节点不是叶子节点，则拷贝y后的孩子到新的分割节点
            if(childNode.isLeaf == false)
            {
                for(int j = 0;j <= splitLength;j++)
                {
                    newSplitTreeNode.childs[j] = childNode.childs[j + splitLength + 1];
                }
            }
            //原节点缩小
            childNode.count = splitLength;
            //当前节点让出位置给新加入的节点
            for(int j = count;j > i;j--)
            {
                childs[j + 1] = childs[j];
            }
            childs[i + 1] = newSplitTreeNode;
            //根节点加入分裂节点的其中的一个节点
            Values[i] = childNode.Values[splitLength];
            count++;
        }
        #endregion
        
        #region 删除
        public void DeleteData(T data)
        {
            int i = 0;
            int t = (order + 1) / 2;
            //获取对应的节点位置
            while(i < count && compareFunc(data,Values[i]) > 0)
            {
                i++;
            }
            if(isLeaf)
            {
                //关键字在叶子节点中，直接删除
                if(Values[i].Equals(data))
                {
                    for(int j = i;j < count;j++)
                    {
                        Values[j] = Values[j + 1];
                    }

                    count--;
                }
            }
            else
            {
                BtreeNode<T> leftNode = childs[i];
                BtreeNode<T> rightNode = null;
                if(i < count)
                {
                    rightNode = childs[i + 1];
                }
                //关键字内部节点中
                if(i  < count && Values[i].Equals(data))
                {
                    //前驱节点数量大于最少数量，则将前驱节点的最后一个数据最为替补删除数据
                    if(leftNode.count > t - 1)
                    {
                        
                    }
                }
            }
        }
        
        /// <summary>
        /// 寻找前驱数据
        /// </summary>
        /// <returns></returns>
        public T FindPreDecessor(BtreeNode<T> node)
        {
            if(node.isLeaf)
            {
                return node.Values[node.count - 1];
            }

            return FindPreDecessor(node.childs[node.count]);
        }
        
        /// <summary>
        /// 获取后继数据
        /// </summary>
        /// <returns></returns>
        public T FindSuccessor(BtreeNode<T> node)
        {
            if(node.isLeaf)
            {
                return node.Values[0];
            }

            return FindSuccessor(node.childs[0]);
        }
        #endregion
        
        #region 查询
        //关键字数量达到下限
        public bool IsMinCount()
        {
            return count == MinCount;
        }
        
        //关键字数量达到上限
        public bool IsMaxCount()
        {
            return count == MaxCount;
        }
        #endregion
    }
}