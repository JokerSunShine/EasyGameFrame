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
        public int count;
        
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
        //父节点
        public BtreeNode<T> parentNode;
        //父节点下节点编号(最后一个是最后元素的右节点，其他都是元素的左节点)
        public int parentChildIndex;
        //归属的树
        private BTree<T> tree;
        #endregion
        
        #region 构造
        public BtreeNode(int order,bool isLeaf,Func<T,T,int> compareFunc,BTree<T> tree,int count = 0)
        {
            this.order = order;
            this.isLeaf = isLeaf;
            this.compareFunc = compareFunc;
            this.count = count;
            values = new T[MaxCount];
            childs = new BtreeNode<T>[MaxCount + 1];
            this.tree = tree;
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
            
            if(count > 0)
            {
                for(int i = 0;i <= count;i++)
                {
                    if(!isLeaf)
                    {
                        childs[i].Traversal(ref traversaList,action);
                        
                        if(i < count)
                        {
                            TraversalAdd(ref traversaList,action);
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
                    if(compareFunc(data,values[nextNodeIndex]) > 0)
                    {
                        nextNodeIndex++;
                    }
                }
                childs[nextNodeIndex].BTreeInserNodeNotFull(data);
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
            childNode.parentNode = this;
            childNode.parentChildIndex = i;
            if(childNode.IsMaxCount() == false)
            {
                return;
            }
            //分割的数据长度
            int splitLength = (int)((order + 1) * 0.5f) - 1;
            //构建一个新的分割节点
            BtreeNode<T> newSplitTreeNode = new BtreeNode<T>(childNode.order,childNode.isLeaf,childNode.compareFunc,tree,splitLength);
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
                BtreeNode<T> leftNode = childs[i];
                BtreeNode<T> rightNode = null;
                if(i < count)
                {
                    rightNode = childs[i + 1];
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
                        leftLeftTreeNode = childs[i - 1];
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
        /// 左子节点转移一个数据到右子节点
        /// </summary>
        /// <param name="i">节点的索引</param>
        public void BTreeLeftToRightChild(int i)
        {
            BtreeNode<T> nowNode = this,leftNode = null,rightNode = null;
            if(i <= 0 || i >= count)
            {
                //没有左节点或没有右节点
                return;
            }
            leftNode = nowNode.childs[i];
            rightNode = nowNode.childs[i + 1];
            //右节点增加数据,父节点数据转移到右节点
            rightNode.count++;
            for(int j = rightNode.count;j > 0;j--)
            {
                rightNode.Values[j] = rightNode.Values[j - 1];
            }
            rightNode.Values[0] = nowNode.Values[i];
            //父节点送给右节点的数据由左节点来替补
            nowNode.Values[i] = leftNode.Values[leftNode.count - 1];
            if(!rightNode.isLeaf)
            {
                //如果不是叶子节点，那么左节点的子节点要给右节点的子节点
                for(int j = rightNode.childs.Length + 1;j > 0;j--)
                {
                    rightNode.childs[j] = rightNode.childs[j - 1];
                }

                rightNode.childs[0] = leftNode.childs[leftNode.count];
            }

            leftNode.count--;
        }
        
        /// <summary>
        /// 右子节点转移一个数据到左子节点
        /// </summary>
        /// <param name="i">节点的索引</param>
        public void BTreeRightToLeftChild(int i)
        {
            BtreeNode<T> nowNode = this,leftNode = null,rightNode = null;
            if(i <= 0 || i >= count)
            {
                //没有左节点或没有右节点
                return;
            }
            leftNode = nowNode.childs[i];
            rightNode = nowNode.childs[i + 1];
            //左节点增加数据,父节点数据转移到左节点
            leftNode.count++;
            for(int j = leftNode.count;j > 0;j--)
            {
                leftNode.Values[j] = leftNode.Values[j - 1];
            }
            leftNode.Values[0] = nowNode.Values[i];
            //父节点送给左节点的数据由右节点来替补
            nowNode.Values[i] = rightNode.Values[rightNode.count - 1];
            if(!leftNode.isLeaf)
            {
                //如果不是叶子节点，那么右节点的子节点要给左节点的子节点
                for(int j = leftNode.childs.Length + 1;j > 0;j--)
                {
                    leftNode.childs[j] = leftNode.childs[j - 1];
                }

                leftNode.childs[0] = rightNode.childs[rightNode.count];
            }

            rightNode.count--;
        }
        
        /// <summary>
        /// 合并节点
        /// </summary>
        /// <param name="i">节点的索引</param>
        public void BTreeNodeMerge(int i)
        {
            BtreeNode<T> nowNode = this;
            if(i >= nowNode.childs.Length)
            {
                return;
            }

            BtreeNode<T> leftNode = nowNode.childs[i],rightNode = null;
            if(i < nowNode.count)
            {
                rightNode = childs[i + 1];
            }

            if(rightNode == null)
            {
                return;
            }
            
            //将右侧节点的所有值放到左侧节点
            int t = (nowNode.order + 1) / 2;
            int startCount = leftNode.count, endCount = startCount + rightNode.count;
            for(int j = startCount;j < endCount;j++)
            {
                leftNode.Values[j + 1] = rightNode.Values[j - startCount];
            }
            leftNode.count += rightNode.count + 1;
            //需要被删除的节点放到中间
            leftNode.Values[t - 1] = nowNode.Values[i];
            //如果左侧节点不是叶子节点，那么右侧节点的所有子节点都需要左侧节点来继承
            if(!leftNode.isLeaf)
            {
                for(int j = t;j < leftNode.count + 1;j++)
                {
                    leftNode.childs[j] = rightNode.childs[j - t];
                }

                BtreeNode<T> node = null;
                for(int j = 0;j < leftNode.count + 1;j++)
                {
                    node = leftNode.childs[j];
                    node.parentNode = leftNode;
                    node.parentChildIndex = j;
                }
            }
            //父节点移除被删除的元素和其相关的子节点
            for(int j = i;j < nowNode.count - 1;j++)
            {
                nowNode.Values[j] = nowNode.Values[j + 1];
            }
            for(int j = i + 1;j < nowNode.count;j++)
            {
                nowNode.childs[j] = nowNode.childs[j + 1];
            }

            nowNode.count--;
            if(nowNode.count <= 0)
            {
                //没有父节点为根节点，根节点则直接将合并的节点替换
                if(nowNode.parentNode == null)
                {
                    tree.Root = leftNode;
                }
                else
                {
                    nowNode.parentNode.childs[nowNode.parentChildIndex] = leftNode;
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