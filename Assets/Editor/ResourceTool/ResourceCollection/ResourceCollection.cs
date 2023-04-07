using System;

namespace ResourceTool.ResourceCollection
{
    public class ResourceColliection
    {
        #region 事件
        public event Action<int, int> OnLoadingResource = null;
        public event Action<int, int> OnLoadingAsset = null;
        public event Action OnLoadCompleted = null;
        #endregion
        
        #region 构造
        public ResourceColliection()
        {
            
        }
        #endregion
    }
}