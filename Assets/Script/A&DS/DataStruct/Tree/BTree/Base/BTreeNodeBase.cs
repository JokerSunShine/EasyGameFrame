using System;
using UnityEngine;
using Common.OneWayChainList;

namespace DataStruct.Tree.BTree.Base
{
    public abstract class BTreeNodeBase<T>
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
        protected int order = 0;
        
        private int minCount = -1;
        //最小关键值数量
        public int MinCount
        {
            get
            {
                if(minCount < 0)
                {
                    minCount =  Mathf.CeilToInt(order * 0.5f) - 1;
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
        public BTreeNodeBase<T>[] childs;
        //是否是子节点
        public bool isLeaf;
        //对比方法
        public Func<T, T, int> compareFunc;
        //父节点
        public BTreeNodeBase<T> parentNode;
        //父节点下节点编号(最后一个是最后元素的右节点，其他都是元素的左节点)
        public int parentChildIndex;
        //归属的树
        protected BTreeBase<T> tree;
        private int middleIndex;
        /// <summary>
        /// 中间节点
        /// </summary>
        protected int MiddleIndex
        {
            get
            {
                if(order <= 0)
                {
                    middleIndex = (order + 1) / 2;
                }

                return middleIndex;
            }
        }
        #endregion
        
        #region 构造
        public BTreeNodeBase(int order,bool isLeaf,Func<T,T,int> compareFunc,BTreeBase<T> tree,int count = 0)
        {
            this.order = order;
            this.isLeaf = isLeaf;
            this.compareFunc = compareFunc;
            this.count = count;
            values = new T[MaxCount];
            childs = new BTreeNodeBase<T>[MaxCount + 1];
            this.tree = tree;
        }
        #endregion
        
        #region 节点分割
        /// <summary>
        /// 当前节点分割指定的子节点
        /// </summary>
        /// <param name="i">分割的子节点下标</param>
        public virtual void SplitChild(int i)
        {
            if(childs == null || i >= childs.Length)
            {
                return;
            }

            BTreeNodeBase<T> childNode = childs[i];
            childNode.parentNode = this;
            childNode.parentChildIndex = i;
            if(childNode.IsMaxCount() == false)
            {
                return;
            }
            //分割的数据长度
            int splitLength = (int)((order + 1) * 0.5f) - 1;
            //构建一个新的分割节点
            BTreeNodeBase<T> newSplitTreeNode = GetNewTreeNoe(childNode.order,childNode.isLeaf,childNode.compareFunc,tree,splitLength);
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
        
        #region 节点转移
        /// <summary>
        /// 左子节点转移一个数据到右子节点
        /// </summary>
        /// <param name="i">节点的索引</param>
        public void BTreeLeftToRightChild(int i,bool useLeftData = false)
        {
            BTreeNodeBase<T> nowNode = this,leftNode = null,rightNode = null;
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

            if (useLeftData)
                rightNode.Values[0] = leftNode.Values[leftNode.count - 1];
            else
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
        public void BTreeRightToLeftChild(int i,bool useRightData = false)
        {
            BTreeNodeBase<T> nowNode = this,leftNode = null,rightNode = null;
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

            if (useRightData)
                leftNode.Values[0] = nowNode.Values[i];
            else
                leftNode.Values[0] = rightNode.Values[rightNode.count - 1];
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
        #endregion
        
        #region 节点合并
        /// <summary>
        /// 合并节点
        /// </summary>
        /// <param name="i">节点的索引</param>
        public void BTreeNodeMerge(int i)
        {
            BTreeNodeBase<T> nowNode = this;
            if(i >= nowNode.childs.Length)
            {
                return;
            }

            BTreeNodeBase<T> leftNode = nowNode.childs[i],rightNode = null;
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

                BTreeNodeBase<T> node = null;
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
        #endregion
        
        #region 获取
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public T GetMaxValue()
        {
            if(count - 1 >= Values.Length)
            {
                return default(T);
            }
            return Values[count - 1];
        }
        #endregion

        public abstract BTreeNodeBase<T> FindNode(T data);
        public abstract void Traversal(ref OneWayChainList<T> traversaList, Action<T> action = null);
        public abstract void InserData(T data);
        public abstract bool IsMinCount();
        public abstract bool IsMaxCount();
        public abstract void DeleteData(T data);
        public abstract BTreeNodeBase<T> GetNewTreeNoe(int order,bool isLeaf,Func<T,T,int> compareFunc,BTreeBase<T> tree,int count = 0);
    }
}