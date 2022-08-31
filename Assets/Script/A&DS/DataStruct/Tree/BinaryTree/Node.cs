using System;

namespace DataStruct.Tree.BinaryTree
{
    public class Node<T>
    {
        #region 枚举
        /// <summary>
        /// 父类节点类型
        /// </summary>
        public enum ParentPointType
        {
            None,
            Left,
            Right,
        }
        #endregion
        
        #region 构造
        private ChainBinaryTreeAbstract<T> tree;
        public ChainBinaryTreeAbstract<T> Tree
        {
            get => tree;
        }
        
        private T data;
        public T Data
        {
            get => data;
        }
        private Node<T> leftNode;
        public Node<T> LeftNode
        {
            get => leftNode;
            set
            {
                leftNode = value;
                NoticeDataChange();
            }
        }

        private Node<T> rightNode;
        public Node<T> RightNode
        {
            get => rightNode;
            set
            {
                rightNode = value;
                NoticeDataChange();
            }
        }

        private Node<T> parent;
        public Node<T> Parent
        {
            get => parent;
            set => parent = value;
        }

        private bool PointTypeChange = true;
        //父类节点类型
        private ParentPointType pointType = ParentPointType.None;
        public ParentPointType PointType
        {
            get
            {
                if(PointTypeChange && tree != null && tree.CompareFunc != null)
                {
                    PointTypeChange = false;
                    int compareValue = Parent.Tree.CompareFunc(Data, Parent.Data);
                    pointType = compareValue > 0 ? ParentPointType.Right :
                        compareValue < 0 ? ParentPointType.Left : ParentPointType.None;
                }
                return pointType;
            }
        }
        
        /// <summary>
        /// 度
        /// </summary>
        public int Degree
        {
            get
            {
                int degree = 0;
                if(LeftNode != null)
                {
                    degree++;
                }
                if(rightNode != null)
                {
                    degree++;
                }

                return degree;
            }
        }
        
        //子树发生变动
        public bool DepthChange = true;
        private int depth;
        //节点最大深度
        public int Depth
        {
            get
            {
                if(DepthChange)
                {
                    DepthChange = false;
                    int leftDepth = LeftNode == null ? 0 : LeftNode.Depth + 1;
                    int rightDepth = RightNode == null ? 0 : RightNode.Depth + 1;
                    depth = Math.Max(leftDepth, rightDepth);
                }

                return depth;
            }
        }
        #region 用于平衡二叉
        private bool BalanceFactorChange = true;
        private int balanceFactor;
        /// <summary>
        /// 平衡因子
        /// </summary>
        public int BalanceFactor{
            get
            {
                if(BalanceFactorChange)
                {
                    BalanceFactorChange = false;
                    int leftDepth = LeftNode != null ? LeftNode.Depth : 0;
                    int rightDepth = RightNode != null ? RightNode.Depth * -1 : 0;
                    balanceFactor = leftDepth + rightDepth;
                }

                return balanceFactor;
            }
        }
        #endregion
        
        #region 用于红黑树
        //红：false，黑：true
        public bool color;
        #endregion
        #endregion
        
        #region 构造
        public Node()
        {
            data = default(T);
            leftNode = null;
            rightNode = null;
            parent = null;
        }
        
        public Node(T data,ChainBinaryTreeAbstract<T> tree = null,Node<T> leftNode = null,Node<T> rightNode = null,Node<T> parent = null)
        {
            this.data = data;
            this.leftNode = leftNode;
            this.rightNode = rightNode;
            this.parent = parent;
            this.tree = tree;
        }
        
        public Node(bool color,Node<T> parent = null,ParentPointType pointType = ParentPointType.None)
        {
            data = default(T);
            leftNode = null;
            rightNode = null;
            this.Parent = parent;
            this.color = color;
            this.pointType = pointType;
        }
        #endregion
        
        #region 功能
        public bool IsEqual(T data)
        {
            return this.data.Equals(data);
        }
        #endregion
        
        #region 数据触发变动
        public void NoticeDataChange()
        {
            PointTypeChange = true;
            BalanceFactorChange = true;
            DepthChange = true;
            if(Parent != null)
            {
                Parent.NoticeDataChange();
            }
        }
        #endregion
    }
}