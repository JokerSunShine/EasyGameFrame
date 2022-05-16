using System.Collections.Generic;
using System.Linq;

namespace Common.TrieSearch
{
    public class TrieNode
    {
        #region 数据
        //父节点
        private TrieNode parent;
        //当前存储的字符
        private char c;
        //计数
        private int occurances;
        public int Occurances => occurances;
        //子节点列表（这里用字典为了方便查找）
        private Dictionary<char,TrieNode> children;
        #endregion

        #region 构造
        public TrieNode(char c,TrieNode node = null)
        {
            this.c = c;
            this.parent = node;
            children = new Dictionary<char, TrieNode>();
        }
        #endregion
  
        #region 获取
        public TrieNode AddChild(char key)
        {
            if(children.ContainsKey(key) == false)
            {
                children[key] = new TrieNode(key,this);
            }

            return children[key];
        }
        
        public TrieNode GetChildNode(char c)
        {
            foreach(var node in children)
            {
                if(c == node.Key)
                {
                    return node.Value;
                }
            }

            return null;
        }
        
        public void AddCount()
        {
            occurances++;
        }
        
        public void ResetCount()
        {
            occurances = 0;
        }
        
        public void RemoveNode(char key)
        {
            children[key] = null;
        }
        
        public TrieNode[] GetTrieNodeArray()
        {
            return children.Values.ToArray();
        }
        
        public string GetString()
        {
            if(parent == null)
            {
                return "";
            }

            return parent.GetString() + c;
        }
        #endregion

        #region 查询
        public bool HaveChildNode()
        {
            return children.Count > 0;
        }
        #endregion
    }
} 