//https://www.cnblogs.com/luanxm/p/10848032.html
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

            count++;
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
        
        /// <summary>
        /// 获取最小值数据
        /// </summary>
        /// <returns></returns>
        public T GetMinData()
        {
            if(min == null)
            {
                return default(T);
            }

            return min.data;
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
            //计算log2(keyNum),floor向上取整
            //例如：log2(13) = 3,向上取整：4
            int maxDegree = (int) Math.Floor(Math.Log(heap.count) / Math.Log(2));
            int D = maxDegree + 1;
            Node<T>[] unionArray = new Node<T>[D + 1];
            
            //不断取出当前斐波那契堆中的最小节点，然后依次进行合并操作
            while(heap.min != null)
            {
                Node<T> node = RemoveMinTree(heap); //取出堆中最小树
                if(node != null)
                {
                    int d = node.degree;
                    while(unionArray[d] != null)
                    {
                        Node<T> sameNode = unionArray[d];
                        if(heap.compareFunc(node.data,sameNode.data) > 0)
                        {
                            //保证截取的最小值比存储的要小
                            Node<T> temp = node;
                            node = sameNode;
                            sameNode = temp;
                        }
                        
                        Link(sameNode,node);
                        unionArray[d] = null;
                        unionArray[node.degree] = node;
                    }

                    unionArray[d] = node;
                }
            }

            heap.min = null;
            
            for(int i  = 0;i < D;i++)
            {
                if(unionArray[i] != null)
                {
                    if(heap.min == null)
                    {
                        heap.min = unionArray[i];
                    }
                    else
                    {
                        AddNode(unionArray[i],heap.min);
                        if(heap.compareFunc(unionArray[i].data,heap.min.data) < 0)
                        {
                            heap.min = unionArray[i];
                        }
                    }
                }
            }

            return heap;
        }
        
        /// <summary>
        /// 移除最小节点
        /// </summary>
        /// <param name="heap"></param>
        public static void RemoveMinNode(FibHeap<T> heap)
        {
            if(heap.min == null)
            {
                return;
            }
            
            //将min下所有的儿子节点都添加到跟链表中
            while(heap.min.child != null)
            {
                Node<T> child = heap.min.child;
                
                RemoveNode(child);
                //如果缓存数据是最后一个数据，则清空子节点,否则右侧节点为链表开头的最新孩子节点
                if(child.right == child)
                {
                    heap.min.child = null;
                }
                else
                {
                    heap.min.child = child.right;
                }
                
                AddNode(child,heap.min);
                child.parent = null;
            }
            
            //将最小节点从链表中移除
            RemoveNode(heap.min);
            //如果min是链表的最后节点，则说明删除的是堆中最后一个数据
            if(heap.min.right == heap.min)
            {
                heap.min = null;
            }
            else
            {
                heap.min = heap.min.right;
                UnionSameDegreeTree(heap);
            }

            heap.count--;
        }
        
        /// <summary>
        /// 减少度
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="degree"></param>
        private static void RenewDegree(Node<T> parent,int degree)
        {
            parent.degree -= degree;
            if(parent.parent != null)
            {
                RenewDegree(parent.parent,degree);
            }
        }
        #endregion
    }
}