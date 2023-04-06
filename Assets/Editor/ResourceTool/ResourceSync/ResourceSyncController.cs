using System;

namespace ResourceTool
{
    public class ResourceSyncController
    {
        #region 事件
        public event Action<int, int> OnLoadingResource = null;
        #endregion
    }
}