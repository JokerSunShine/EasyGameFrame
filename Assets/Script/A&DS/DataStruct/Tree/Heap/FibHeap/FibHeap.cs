using System;

namespace DataStruct.Tree.Heap.FibHeap
{
    public class FibHeap<T>
    {
        #region 数据
        //节点总数
        private int count;
        //最小节点
        private Node<T> min;
        //对比方法
        private Func<T, T, int> compareFunc;
        #endregion
        
        #region 构造
        public FibHeap(Func<T,T,int> compareFunc,T[] array)
        {
            this.compareFunc = compareFunc;
            foreach(var data in array)
            {
                Insert(data);
            }
        }
        #endregion

        #region 功能
        /// <summary>
        /// 插入节点
        /// </summary>
        /// <param name="node"></param>
        public void Insert(Node<T> node)
        {
            if(count == 0)
            {
                min = node;
            }
            else
            {
                AddNode(node,min);
                if (compareFunc(node.data, min.data) < 0)
                    min = node;
            }
        }
        
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="data"></param>
        public void Insert(T data)
        {
            Insert(new Node<T>(data));
        }
        
        /// <summary>
        /// 合并斐波那契堆
        /// </summary>
        public void Union(FibHeap<T> other)
        {
            if(other == null || other.min == null)
            {
                return;
            }
            
            if(min == null)
            {
                min = other.min;
                count = other.count;
            }
            else
            {
                CatList(this.min,other.min);
                if(compareFunc(this.min.data,other.min.data) >= 0)
                {
                    this.min = other.min;
                }

                count += other.count;
            }
        }
        
        
        #endregion
        
        #region 静态功能
        /// <summary>
        /// 添加节点
        /// a <-> root
        /// a <-> node <-> root
        /// </summary>
        /// <param name="node"></param>
        /// <param name="root"></param>
        public static void AddNode(Node<T> node,Node<T> root)
        {
            node.left = root.left;
            node.right = root;
            root.left.right = node;
            root.left = node;
        }
        
        /// <summary>
        /// 合并两个双向链表
        /// <-> a <-> b <-> ...
        /// <-> c <-> d <-> ...
        /// <-> a <-> d ... <-> c <-> b ... <->
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void CatList(Node<T> a,Node<T> b)
        {
            Node<T> tmp = a.right;
            a.right = b.right;
            b.right.left = a;
            b.right = tmp;
            tmp.left = b;
        }
        
        /// <summary>
        /// 合并两个树
        /// </summary>
        public static void Link(Node<T> node,Node<T> root)
        {
            RemoveNode(node);
            if(root.child == null)
            {
                root.child = node;
            }
            else
            {
                AddNode(node,root.child);
            }

            node.parent = root;
            root.degree++;
            node.marked = false;
        }
        
        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="node"></param>
        public static void RemoveNode(Node<T> node)
        {
            node.left.right = node.right;
            node.right.left = node.left;
        }
        
        /// <summary>
        /// 获取并移除堆中最小树
        /// </summary>
        public static Node<T> RemoveMinTree(FibHeap<T> heap)
        {
            if(heap == null || heap.min == null)
            {
                return null;
            }

            Node<T> min = heap.min;
            if(min == min.right)
            {
                heap.min = null;
            }
            else
            {
                RemoveNode(heap.min);
                heap.min = min.right;
            }

            min.left = min.right = min;
            return min;
        }
        
        public static FibHeap<T> UnionSameDegreeTree(FibHeap<T> heap)
        {
            int maxDegree = (int) Math.Floor(Math.Log(heap.count) / Math.Log(2));
            return null;
        }
        #endregion
        
    }
}