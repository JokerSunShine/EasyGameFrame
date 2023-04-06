using System;
using UnityEngine;

namespace ResourceTool.Base
{
    public class ResourcePageItem
    {
        #region 数据
        public string Name
        {
            get
            {
                if (attribute != null)
                    return attribute.menuName;
                return string.Empty;

            }
        }
        private ResourcePageAttribute attribute;
        private Type curClass;
        public ResourceToolBase tool;
        public int priority
        {
            get
            {
                return attribute.order;
            }
        }
        #endregion
        
        #region 构造
        public ResourcePageItem(ResourcePageAttribute attribute,Type curClass)
        {
            this.attribute = attribute;
            this.curClass = curClass;
        }
        #endregion
        
        #region 生命周期
        public void OnSelect()
        {
            if(tool == null)
            {
                tool = ScriptableObject.CreateInstance(curClass) as ResourceToolBase;
                tool.Init();
            }
            tool.OnRefresh();
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
        #endregion
    }
}