using System;
using Script.Packages.FW_Resource.Base;

namespace Script.Packages.FW_Core.Src
{
    public class ResourceManager
    {
        #region 数据
        public const float ASSET_CACHE_DEFAULT_TIME = -1;
        #endregion
        
        #region Asset
        public static RequestAsset LoadAsset(string path,Type type)
        {
            return Main.instance.resource.LoadAsset(path, type);
        }
        
        public static RequestAsset LoadAssetAsync(string path,Type type)
        {
            return Main.instance.resource.LoadAssetAsync(path, type);
        }
        #endregion
        
        #region Instantiate
        
        #endregion
    }
}