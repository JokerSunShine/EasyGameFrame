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
            set
            {
                Id = value;
            }
        }
        #endregion
        
        #region 数据
        private MapPageAttribute attribute;
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
        public PageTreeViewItem(MapPageAttribute attribute,Type curClass)
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
            tool.OnRefresh();
        }
        
        public void OnGUI(Rect rect)
        {
            if(tool != null)
            {
                tool.OnDrawGUI(rect);
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