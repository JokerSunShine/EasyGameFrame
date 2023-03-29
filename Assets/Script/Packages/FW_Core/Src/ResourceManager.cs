using System;
using Script.Packages.FW_Resource.Base;
using UnityEngine;

namespace Framework
{
    public class ResourceManager
    {
        #region 数据
        public const float ASSET_CACHE_DEFAULT_TIME = -1;
        #endregion
        
        #region Asset
        public static AssetRequest LoadAsset(string path,Type type)
        {
            return Main.instance.resource.LoadAsset(path, type);
        }
        
        public static AssetRequest LoadAssetAsync(string path,Type type)
        {
            return Main.instance.resource.LoadAssetAsync(path, type);
        }
        #endregion
        
        #region Instantiate
        public static GameObject Instantiate(string path, Transform parent = null, bool worldPositionStays = true)
        {
            return Main.instance.resource.Instantiate(path, parent, worldPositionStays);
        }
        
        public static GameObject Instantiate(string path, Vector3 position, Quaternion quaternion, Transform parent = null,
            bool worldPositionStays = true)
        {
            return Main.instance.resource.Instantiate(path, position,quaternion, parent);
        }
        
        public static InstantieteRequest InstantiateAsync(string path, Transform parent = null, bool worldPositionStays = true)
        {
            return Main.instance.resource.InstantiateAsync(path, parent, worldPositionStays);
        }
        
        public static InstantieteRequest InstantiateAsync(string path, Vector3 position, Quaternion quaternion, Transform parent = null,
            bool worldPositionStays = true)
        {  
            return Main.instance.resource.InstantiateAsync(path, position,quaternion, parent);
        }
        #endregion
    }
}