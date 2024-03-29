namespace Common.BothWayLoopChainList
{
    //双向循环链表
    public class BothWayLoopChianList<T>
    {
        #region 数据
        private Node<T> head;
        public Node<T> Head
        {
            get
            {
                if(head == null)
                {
                    head = new Node<T>();
                }
                return head;
            }
        }
        
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        public T this[int index]
        {
            get
            {
                return GetNodeValue(index);
            }
            set
            {
                Insert(value,index);
            }
            
        }

        private int count = 0;
        public int Count
        {
            get
            {
                return count;
            }
        }
        #endregion

        #region 构造
        public BothWayLoopChianList()
        {
            Clear();
        }
        
        public BothWayLoopChianList(T[] array)
        {
            foreach(T data in array)
            {
                Append(data);
            }
        }
        #endregion

        #region 获取
        private Node<T> GetNode(int i)
        {
            if(IsEmpty() || i < 0 || i > Count)
            {
                return null;
            }
            int middleIndex = Count / 2; 
            int index = 0;
            Node<T> node = Head;
            if(i < middleIndex)
            {
                while(index <= i)
                {
                    if(node == null || node.Next == null)
                    {
                        return null;
                    }

                    node = node.Next;
                    index++;
                }

                return node;
            }
            else
            {
                int calculationNum = Count - i;
                while(index <= calculationNum)
                {
                    if(node == null || node.Prev == null)
                    {
                        return null;
                    }

                    node = node.Prev;
                    index++;
                }
            }
            return node;
        }
        public T GetNodeValue(int i)
        {
            Node<T> node = GetNode(i);
            if(node != null && node.HaveData())
            {
                return node.Data;
            }
            return default(T);
        }
        #endregion
 
        #region 查询
        public bool IsEmpty()
        {
            return Head == null || Head.HaveData() == false;
        }
        #endregion

        #region 功能
        public void Append(T item)
        {
            Node<T> node = GetNode(Count);
            if(node == null)
            {
                node = Head;
            }

            node.AddNext(item);
            count++;
        }
        
        public void Insert(T item,int i = -1)
        {
            if(i < 0)
            {
                Append(item);
            }

            Node<T> node = GetNode(i - 1);
            if(node == null || node.HaveData() == false)
            {
                return;
            }

            node.AddNext(item);
            count++;
        }
        
        public void Delete(int i)
        {
            if(i < 1)
            {
                Clear();
                return;
            }
            Node<T> node = GetNode(i - 1);
            if(node == null || node.HaveData() == false || node.HaveNextData() == false)
            {
                return;
            }
            
            Node<T> deleteNextNode = node.Next.Next;
            node.Next = deleteNextNode;
            deleteNextNode.Prev = node;
            count--;
        }
        
        public void Merge(BothWayLoopChianList<T> list)
        {
            Node<T> node = list.Head;
            int index = 0;
            while(index < list.Count)
            {
                if(node == null || node.Data == null)
                {
                    return;
                }
                Append(node.Data);
                node = node.Next;
                index++;
            }
        }
        
        public void Clear()
        {
            head = null;
            count = 0;
        }
        #endregion
    }
}