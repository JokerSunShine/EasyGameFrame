using System;
using System.Collections.Generic;
using System.Reflection;
using ResourceTool.Base;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace ResourceTool
{
    public class ResourceToolsWindow : EditorWindow
    {
        #region 数据
        private static Rect windowRect = new Rect(0,0,1000,700);
        private List<ResourcePageItem> pages = new List<ResourcePageItem>();
        private string[] toogleNames;
        private int toolIndex = 0;
        private ResourcePageItem selectItem;
        #endregion
        
        #region 初始化
        [MenuItem("Tools/ResourceTool",priority = 2)]
        public static void Init()
        {
            ResourceToolsWindow window = GetWindowWithRect<ResourceToolsWindow>(windowRect, false, "ResourceTool");
            window.InitMenus();
        }
        
        public void InitMenus()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            for(int i = 0;i < types.Length;i++)
            {
                ResourcePageAttribute attribute = types[i].GetCustomAttribute<ResourcePageAttribute>();
                if(attribute != null)
                {
                    pages.Add(new ResourcePageItem(attribute,types[i]));   
                }
            }
            pages.Sort(pageSort);
            RefreshToogleNames();
        }
        
        private int pageSort(ResourcePageItem a,ResourcePageItem b)
        {
            return a.priority.CompareTo(b.priority);
        }
        
        private void RefreshToogleNames()
        {
            toogleNames = new string[pages.Count];
            for(int i = 0;i < toogleNames.Length;i++)
            {
                toogleNames[i] = pages[i].Name;
            }
        }
        #endregion
        
        #region Toogle
        public void OnGUI()
        {
            RefreshToogle();
            RefreshSelect();
        }
        
        private void RefreshToogle()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorUtil.CommonSpace);
            toolIndex = GUILayout.Toolbar(toolIndex, toogleNames, "LargeButton", GUILayout.Width(position.width));
            GUILayout.EndHorizontal();
        }
        
        private void RefreshSelect()
        {
            ResourcePageItem item = GetPageItem(toolIndex);
            if(selectItem == null || selectItem.priority != item.priority)
            {
                selectItem = item;
                selectItem.OnSelect();
            }
            if(selectItem != null)
                selectItem.OnGUI();
        }
        
        private ResourcePageItem GetPageItem(int toolIndex)
        {
            if (toolIndex < 0 || toolIndex >= pages.Count)
                return null;
            return pages[toolIndex];
        }
        #endregion
        
        #region 销毁
        private void OnDestroy()
        {
            foreach(var item in pages)
            {
                item.OnDestroy();
            }
        }
        #endregion
    }
}