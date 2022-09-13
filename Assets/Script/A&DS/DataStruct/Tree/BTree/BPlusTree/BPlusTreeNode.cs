using System;
using Common.OneWayChainList;
using DataStruct.Tree.BTree.Base;

namespace DataStruct.Tree.BTree.BPlusTree
{
    public class BPlusTreeNode<T>:BTreeNodeBase<T>
    {
        #region 数据
        public BPlusTreeNode<T> nextNode;
        #endregion
        
        #region 构造
        public BPlusTreeNode(int order,bool isLeaf,Func<T,T,int> compareFunc,BTreeBase<T> tree,int count = 0):base(order,isLeaf,compareFunc,tree,count)
        {
            
        }
        #endregion

        #region 查找
        public override BTreeNodeBase<T> FindNode(T data)
        {
            if(compareFunc == null)
            {
                return null;
            }

            int i = 0;
            while (i < count && compareFunc(data,Values[i]) > 0)
            {
                i++;
            }
            
            if(isLeaf)
            {
                if(data.Equals(Values[i]))
                {
                    return this;
                }

                return null;
            }

            return childs[i].FindNode(data);
        }
        #endregion
        
        #region 遍历
        public override void Traversal(ref OneWayChainList<T> traversaList, Action<T> action = null)
        {
            if(isLeaf)
            {
                for(int i = 0;i < count;i++)
                {
                    traversaList.Append(Values[i]);
                    if (action != null)
                        action(Values[i]);
                }
                if(nextNode != null)
                {
                    nextNode.Traversal(ref traversaList,action);
                }
            }
            else
            {
                BPlusTreeNode<T> leftNode = GetLeftNode();
                if(leftNode != null)
                    leftNode.Traversal(ref traversaList,action);
            }
        }
        
        private BPlusTreeNode<T> GetLeftNode()
        {
            if(isLeaf)
            {
                return this;
            }
            if(childs.Length <= 0)
            {
                return null;
            }
            return ((BPlusTreeNode<T>) childs[0]).GetLeftNode();
        }
        #endregion

        #region 插入
        public override void InserData(T data)
        {
            int i = count - 1;
            
            if(isLeaf)
            {
                while(i >= 0 && compareFunc(Values[i],data) > 0)
                {
                    Values[i + 1] = Values[i];
                    i--;
                }

                Values[i + 1] = data;
                count++;
                //刷新父节点索引
                RefreshParentMaxValue();
            }
            else
            {
                while(i >= 0 && compareFunc(Values[i],data) > 0)
                {
                    i--;
                }

                int nextNodeIndex = i == count - 1 ? i : ++i;
                if(childs[nextNodeIndex].IsMaxCount())
                {
                    SplitChild(i);
                    if(compareFunc(data,Values[nextNodeIndex]) > 0)
                    {
                        nextNodeIndex++;
                    }
                }
                childs[nextNodeIndex].InserData(data);
                if(parentNode != null)
                {
                    if(IsMaxCount())
                    {
                        SplitChild(parentChildIndex);
                    }
                }
            }
        }
        
        /// <summary>
        /// 刷新父类节点最大值
        /// </summary>
        private void RefreshParentMaxValue()
        {
            if(parentNode == null)
            {
                return;
            }

            T maxValue = Values[count - 1];
            if(compareFunc(parentNode.Values[parentChildIndex],maxValue) != 0)
            {
                parentNode.Values[parentChildIndex] = maxValue;
                ((BPlusTreeNode<T>)parentNode).RefreshParentMaxValue();
            }
        }
        
        /// <summary>
        /// 当前节点分割指定的子节点
        /// </summary>
        /// <param name="i">分割的子节点下标</param>
        public override void SplitChild(int i)
        {
            if(childs == null || i >= childs.Length)
            {
                return;
            }

            BPlusTreeNode<T> childNode = (BPlusTreeNode<T>)childs[i];
            childNode.parentNode = this;
            childNode.parentChildIndex = i;
            if(childNode.IsMaxCount() == false)
            {
                return;
            }
            //分割的数据长度
            int splitLength = (int)((order + 1) * 0.5f) - 1;
            //构建一个新的分割节点
            BPlusTreeNode<T> newSplitTreeNode = new BPlusTreeNode<T>(order,childNode.isLeaf,childNode.compareFunc,tree,splitLength);
            //连接到下一个节点
            childNode.nextNode = newSplitTreeNode;
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
            //设置父节点
            newSplitTreeNode.parentNode = this;
            newSplitTreeNode.parentChildIndex = i + 1;
            //原节点缩小
            childNode.count = splitLength + 1;
            //当前节点让出位置给新加入的节点
            for(int j = count - 1;j > i;j--)
            {
                childs[j + 1] = childs[j];
            }
            count++;
            childs[i + 1] = newSplitTreeNode;
            //根节点加入分裂节点的其中的一个节点
            childNode.RefreshParentMaxValue();
            newSplitTreeNode.RefreshParentMaxValue();
        }
        #endregion
        
        #region 删除
        public override void DeleteData(T data)
        {
            int i = 0;
            //获取对应的节点位置
            while(i < count && compareFunc(data,Values[i]) > 0)
            {
                i++;
            }
            if(isLeaf)
            {
                 if(Values[i].Equals(data))
                 {
                     for(int j = i;j < count - 1;j++)
                     {
                         Values[j] = Values[j + 1];
                     }
                 }
                 count--;
                 if(i == count)
                 {
                     RefreshParentMaxValue();
                 }
            }
            else
            {
                BPlusTreeNode<T> childNode = (BPlusTreeNode<T>)childs[i];
                if(childNode.isLeaf)
                {
              
                    BPlusTreeNode<T> childRightnode = null,childLeftNode = null;
                    if(i < count)
                    {
                        childRightnode = (BPlusTreeNode<T>) childs[i + 1];
                    }
                    if(i > 0)
                    {
                        childLeftNode = (BPlusTreeNode<T>) childs[i - 1];
                    }
                    if(childNode.count <= MiddleIndex - 1)
                    {
                        if(childLeftNode != null && childLeftNode.count > MiddleIndex - 1)
                        {
                            BTreeLeftToRightChild(i - 1,true);
                        }
                        else if(childRightnode != null && childRightnode.count > MiddleIndex - 1)
                        {
                            BTreeRightToLeftChild(i,true);
                        }
                        else if(i > 0)
                        {
                            BTreeNodeMerge(i - 1);
                            if(childLeftNode != null)
                                childLeftNode.nextNode = childRightnode;
                        }
                        else
                        {
                            BTreeNodeMerge(i);
                            if (childRightnode != null)
                                nextNode = childRightnode.nextNode;
                        }
                        RefreshParentMaxValue();
                    }
                }
                childNode.DeleteData(data);
            }
        }
        #endregion

        #region 查询
        public override bool IsMinCount()
        {
            return count == MinCount;
        }
        
        public override bool IsMaxCount()
        {
            return count == MaxCount;
        }
        #endregion

        #region 获取子节点
        public override BTreeNodeBase<T> GetNewTreeNoe(int order, bool isLeaf, Func<T, T, int> compareFunc, BTreeBase<T> tree, int count = 0)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}