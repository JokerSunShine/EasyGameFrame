using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace MapTools.Page
{
    public class PageTreeView : TreeView
    {
        #region 数据
        private TreeViewItem root;
        private List<PageTreeViewItem> menus;
        private IPageSelect pageSelect;
        private int selectId;
        #endregion
        
        #region 构造
        public PageTreeView(TreeViewState state,List<PageTreeViewItem> menus,IPageSelect select):base(state)
        {
            this.menus = menus;
            this.pageSelect = select;
            Reload();
        }
        
        public void RefreshFirstPage()
        {
            if(menus == null || menus.Count <= 0)
            {
                return;
            }

            int firstId = menus[0].priority;
            SelectionChanged(new List<int>(firstId));
        }
        #endregion
        
        protected override TreeViewItem BuildRoot()
        {
            if(root == null)
            {
                root = new TreeViewItem(-1,-1,"Root");
            }
            foreach(PageTreeViewItem item in menus)
            {
                root.AddChild(item);
            }
            return root;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            selectId = selectedIds[selectedIds.Count - 1];
            PageTreeViewItem viewItem = FindItem(selectId,root) as PageTreeViewItem;
            if(viewItem != null)
                pageSelect.OnSelectChanged(viewItem);
        }
    }
}