using System;
using Common.OneWayChainList;
using DataStruct.Tree.BTree.Base;

namespace DataStruct.Tree.BTree.BTree
{
    public class BtreeNode<T>:BTreeNodeBase<T>
    {
        #region 构造
        public BtreeNode(int order,bool isLeaf,Func<T,T,int> compareFunc,BTreeBase<T> tree,int count = 0):base(order,isLeaf,compareFunc,tree,count)
        {
           
        }
        #endregion
        
        #region 功能
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override BTreeNodeBase<T> FindNode(T data)
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
        public override void Traversal(ref OneWayChainList<T> traversaList,Action<T> action = null)
        {
            if(Values == null || count <= 0)
            {
                return;
            }
            
            if(count > 0)
            {
                for(int i = 0;i <= count;i++)
                {
                    if(!isLeaf)
                    {
                        childs[i].Traversal(ref traversaList,action);
                        
                        if(i < count)
                        {
                            T nowData = Values[i];
                            if(traversaList != null)
                            {
                                traversaList.Append(nowData);
                            }
                
                            if(action != null)
                            {
                                action(nowData);
                            }
                        }
                    }
                    if(isLeaf && i == 0)
                    {
                        TraversalAdd(ref traversaList,action);
                    }
                }
            }
        }
        
        private void TraversalAdd(ref OneWayChainList<T> traversaList,Action<T> action = null)
        {
            T nowData;
            for(int j = 0;j < count;j++)
            {
                nowData = Values[j];
                if(traversaList != null)
                {
                    traversaList.Append(nowData);
                }
                
                if(action != null)
                {
                    action(nowData);
                }
            }
        }
        #endregion
        
        #region 加入数据
        /// <summary>
        /// 未满数据插入
        /// </summary>
        /// <param name="data">插入的数据</param>
        public override void InserData(T data)
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
                    if(compareFunc(data,Values[nextNodeIndex]) > 0)
                    {
                        nextNodeIndex++;
                    }
                }
                childs[nextNodeIndex].InserData(data);
                //如果由于下层节点分割导致当前节点满了，则当前节点也需要分割
                if(parentNode != null)
                {
                    if(IsMaxCount())
                    {
                        SplitChild(parentChildIndex);
                    }
                }
            }
        }
        #endregion

        public override BTreeNodeBase<T> GetNewTreeNoe(int order,bool isLeaf,Func<T,T,int> compareFunc,BTreeBase<T> tree,int count = 0)
        {
            return new BtreeNode<T>(order,isLeaf,compareFunc,tree,count);
        }
        
        #region 删除
        public override void DeleteData(T data)
        {
            int i = 0;
            //中间节点位置
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
                    for(int j = i;j < count - 1;j++)
                    {
                        Values[j] = Values[j + 1];
                    }

                    count--;
                }
            }
            else
            {
                BtreeNode<T> leftNode = (BtreeNode<T>)childs[i];
                BtreeNode<T> rightNode = null;
                if(i < count)
                {
                    rightNode = (BtreeNode<T>)childs[i + 1];
                }
                //关键字在当前节点中
                if(i  < count && Values[i].Equals(data))
                {
                    //前驱节点数量大于最少数量，则将前驱节点的最后一个数据最为替补删除数据
                    if(leftNode.count > t - 1)
                    {
                        T decessorData = FindPreDecessor(leftNode);
                        DeleteData(decessorData);
                        Values[i] = decessorData;
                    }
                    else if(rightNode != null && rightNode.count > t - 1)
                    {
                        //如果前继节点数量不够，则从后继节点的第一个数据作为替补删除数据
                        T successorData = FindSuccessor(rightNode);
                        DeleteData(successorData);
                        Values[i] = successorData;
                    }
                    else
                    {
                        //如果左侧节点和右侧节点都没有可替补的元素，则将左右节点进行合并，并将元素删除
                        BTreeNodeMerge(i);
                        leftNode.DeleteData(data);
                    }
                }
                else
                {
                    //关键字在下一级的子树中
                    //如果子树的数量不足不足，则
                    //1.如果左兄弟富足，则从左兄弟转移一个元素过来
                    //2.如果右兄弟富足，则从右兄弟转移一个元素过来
                    //3.如果两边都不富足，如果有左节点，则合并左节点，否则合并右节点
                    BtreeNode<T> leftLeftTreeNode = null;
                    if(i > 0)
                    {
                        leftLeftTreeNode = (BtreeNode<T>)childs[i - 1];
                    }
                    //左节点为当前探索的下一个节点，如果探索节点不满足最少节点，则对齐进行补足处理
                    if(leftNode.count <= t - 1)
                    {
                        if(leftLeftTreeNode != null && leftLeftTreeNode.count > t - 1)
                        {
                            //左左节点富裕，则从左左节点取一个点补足探索节点
                            BTreeLeftToRightChild(i - 1);
                        }
                        else if(rightNode != null && rightNode.count > t - 1)
                        {
                            //右节点赋予，则从右节点取一个不足探索节点
                            BTreeRightToLeftChild(i);
                        }
                        else if(i > 0)
                        {
                            //如果有左左节点，则将左左节点进行合并
                            BTreeNodeMerge(i - 1);
                            leftNode = leftLeftTreeNode;
                        }
                        else
                        {
                            //如果有右节点，则将右节点进行合并
                            BTreeNodeMerge(i);
                        }
                    }
                    if(leftNode != null)
                    {
                        leftNode.DeleteData(data);
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

            return FindPreDecessor((BtreeNode<T>)node.childs[node.count]);
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

            return FindSuccessor((BtreeNode<T>)node.childs[0]);
        }
        #endregion
        
        #region 查询
        //关键字数量达到下限
        public override bool IsMinCount()
        {
            return count == MinCount;
        }
        
        //关键字数量达到上限
        public override bool IsMaxCount()
        {
            return count == MaxCount;
        }
        #endregion
    }
}