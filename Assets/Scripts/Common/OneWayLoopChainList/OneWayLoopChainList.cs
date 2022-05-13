namespace Common.OneWayLoopChainList
{
    public class OneWayChainList<T>
    {
        //单链表
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
        
        public int Count
        {
            get
            {
                return GetLength();
            }
        }
        #endregion

        #region 构造
        public OneWayChainList()
        {
            Clear();
        }
        
        public OneWayChainList(T[] array)
        {
            foreach(T data in array)
            {
                Append(data);
            }
        }
        #endregion

        #region 获取
        public int GetLength()
        {
            if(IsEmpty())
            {
                return 0;
            }
            Node<T> node = Head;
            int length = 0;
            while(node != null && node.HaveData())
            {
                node = node.Next;
                length++;
            }

            return length;
        }
        
        private Node<T> GetNode(int i)
        {
            if(IsEmpty() || i < 0 || i > GetLength())
            {
                return null;
            }

            Node<T> node = Head;
            int index = 0;
            while(node.HaveNextData() && index < i)
            {
                node = node.Next;
                index++;
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
            Node<T> node = Head;
            while(node.HaveNextData())
            {
                node = node.Next;
            }
            node.AddNext(item);
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

            Node<T> nextNode = node.Next;
            Node<T> newNextNode = node.AddNext(item);
            newNextNode.AddNext(nextNode);
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
            node.AddNext(deleteNextNode);
        }
        
        public void Reverse()
        {
            if(Count == 1 || Head == null)
            {
                return;
            }
            
            Node<T> NewHead = null;
            Node<T> NowNode = Head;
            Node<T> ParentNode;

            while (NowNode != null)
            {
                ParentNode = NowNode.Next;
                NowNode.AddNext(NewHead);
                NewHead = NowNode;
                NowNode = ParentNode;
            }

            head = NewHead;
        }
        
        public T GetMiddleValue()
        {
            Node<T> A = Head;
            Node<T> B = Head;
            
            while(B != null && B.Next != null)
            {
                A = A.Next;
                B = B.Next;
            }
            
            if(B == null)
            {
                //奇数
                return A.Data;
            }
            else
            {
                //偶数
                return A.Data;
            }
        }
        
        public void Merge(OneWayChainList<T> list)
        {
            Node<T> node = list.Head;
            while(node != null && node.Data != null)
            {
                Append(node.Data);
                node = node.Next;
            }
        }
        
        public void Clear()
        {
            head = null;
        }
        #endregion
    }
}