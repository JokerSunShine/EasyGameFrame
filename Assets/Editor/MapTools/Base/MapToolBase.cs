using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MapTools.Base
{
    public class MapToolBase : EditorWindow
    {
        #region 数据
        private string titleName;
        private Rect ToolShowRect = Rect.zero;
        private GUIStyle titleFontStyle;
        private Vector2 scrollPosition;
        #endregion
        
        #region 构造
        public MapToolBase()
        {
            PageAttribute page = GetType().GetCustomAttribute<PageAttribute>();
            if(page != null)
            {
                titleName = string.Format("<color=#D2D2D2>{0}</color>", page.menuName);

            }
        }
        #endregion
        
        #region 初始化
        public virtual void Init()
        {
            titleFontStyle = new GUIStyle();
            titleFontStyle.richText = true;
            titleFontStyle.fontStyle = FontStyle.Bold;
            titleFontStyle.fontSize = 20;
        }
        #endregion

        #region 刷新
        public virtual void OnRefresh(){}
        #endregion
       
        #region GUI
        public void OnDrawGUI(Rect rect)
        {
            ToolShowRect.Set(rect.width + 10,rect.y + 10,1000 - rect.width,rect.height);
            GUILayout.BeginArea(ToolShowRect);
            GUILayout.BeginVertical();
            {
                GUILayout.Label(titleName,titleFontStyle);
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                OnToolGUI();
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        
        public virtual void OnToolGUI()
        {
            
        }
        #endregion
 
        #region 关闭
        public virtual void OnClose(){}
        #endregion
    }
}