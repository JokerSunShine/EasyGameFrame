using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ResourceTool.Base
{
    public abstract class ResourceToolBase : EditorWindow
    {
        #region 数据
        private ResourcePageAttribute mapPage;
        #endregion
        
        #region 构造
        public ResourceToolBase()
        {
            mapPage = GetType().GetCustomAttribute<ResourcePageAttribute>();
        }
        #endregion
        
        #region 初始化
        public virtual void Init(){}
        #endregion

        #region 刷新
        public virtual void OnRefresh(){}
        #endregion
       
        #region GUI
        public void OnDrawGUI()
        {
            OnToolGUI();
        }
        
        public virtual void OnToolGUI(){}
        #endregion
 
        #region 关闭
        public virtual void OnClose(){}
        #endregion
    }
}