using System;
using System.Collections.Generic;
using System.Reflection;
using MapTools.Base;
using MapTools.Page;
using Unity.Burst;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MapTools
{
    public class MapToolsWindow : EditorWindow,IPageSelect
    {
        #region 数据
        private static Rect windowRect = new Rect(0,0,700,700);
        private Rect treeViewRect = new Rect(0,0,150,700);
        private Rect splitLineRect = new Rect(150,0,1,700);
        private Rect splitEventLineRect = new Rect(145,0,10,700);
        private static List<PageTreeViewItem> pageTreeViewItemList = new List<PageTreeViewItem>();
        private TreeViewState treeViewState;
        private PageTreeView treeView;
        private PageTreeViewItem selectPage;
        #endregion
        
        #region 初始化
        [MenuItem("Tools/MapTool",priority = 2)]
        public static void Init()
        {
            MapToolsWindow window = GetWindowWithRect<MapToolsWindow>(windowRect,false ,"MapTools");
            window.InitMenus();
        }
        
        public void InitMenus()
        {
            PageTreeViewItem.ID = 0;
            pageTreeViewItemList.Clear();

            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            for(int i = 0;i < types.Length;i++)
            {
                PageAttribute attribute = types[i].GetCustomAttribute<PageAttribute>();
                if(attribute != null)
                {
                    pageTreeViewItemList.Add(new PageTreeViewItem(attribute,types[i]));
                }
            }
            pageTreeViewItemList.Sort(MenusSort);
            treeView = new PageTreeView(new TreeViewState(), pageTreeViewItemList,this);
            treeView.RefreshFirstPage();
        }
        
        public int MenusSort(PageTreeViewItem pageTreeViewItemA,PageTreeViewItem pageTreeViewItemB)
        {
            return pageTreeViewItemA.priority.CompareTo(pageTreeViewItemB.priority);
        }
        #endregion

        public void OnSelectChanged(PageTreeViewItem item)
        {
            selectPage = item;
            selectPage.OnSelect();
        }

        private void OnGUI()
        {
            treeView.OnGUI(treeViewRect);
            if(selectPage != null && selectPage.tool != null)
            {
                selectPage.OnGUI(treeViewRect);
            }

            DrawSpliterLine();
        }
        
        private void DrawSpliterLine()
        {
            AddSpliterLineEvent();
            Color col = GUI.color;
            GUI.color = new Color(0.3f, 0.3f, 0.3f);
            splitLineRect.Set(treeViewRect.width, treeViewRect.y, splitLineRect.width, splitLineRect.height);
            GUI.DrawTexture(splitLineRect, EditorGUIUtility.whiteTexture);
            GUI.color = col;
        }

        private void AddSpliterLineEvent()
        {
            splitEventLineRect.Set(treeViewRect.width - 5, treeViewRect.y, splitEventLineRect.width, splitEventLineRect.height);
            EditorUtil.AddSpliterLineEvent(splitEventLineRect,MouseCursor.ResizeHorizontal,TreeViewMouseDown);
        }
        
        private void TreeViewMouseDown(Vector3 vec)
        {
            treeViewRect.width = vec.x;
            Repaint();
        }
        
        private void OnDestroy()
        {
            foreach(var item in pageTreeViewItemList)
            {
                item.OnDestroy();
            }
        }
    }
}