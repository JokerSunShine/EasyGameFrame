using System;
using MapTools.Base;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MapTools.Page
{
    public class PageTreeViewItem : TreeViewItem
    {
        #region 静态数据
        private static int Id = 0;
        public static int ID
        {
            get
            {
                Id++;
                return Id;
            }
        }
        #endregion
        
        #region 数据
        private PageAttribute attribute;
        private Type curClass;
        public MapToolBase tool;
        public int priority
        {
            get
            {
                return attribute.layer;
            }
        }
        #endregion
        
        #region 构造
        public PageTreeViewItem(PageAttribute attribute,Type curClass)
        {
            this.attribute = attribute;
            this.curClass = curClass;
            this.displayName = attribute.menuName;
            this.id = ID;
        }
        #endregion
        
        public void OnSelect()
        {
            if(tool == null)
            {
                tool = ScriptableObject.CreateInstance(curClass) as MapToolBase;
                tool.Init();
            }
            tool.OnSelect();
        }
        
        public void OnGUI()
        {
            if(tool != null)
            {
                tool.OnDrawGUI();
            }
        }
        
        public void OnDestroy()
        {
            if(tool != null)
            {
                tool.OnClose();
            }
        }
    }
}