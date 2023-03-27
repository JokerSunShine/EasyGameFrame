using System;
using System.Collections;
using Script.Packages.FW_Core.Src;
using Script.Packages.FW_Resource.Base;
using Script.Packages.FW_Resource.ResourceLoad;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.Packages.FW_Resource.Task
{
    public class AssetTask : TaskBase
    {
        #region 数据
        public Type type;
        
        public Object asset
        {
            get
            {
                return res as Object;
            }
        }

        public float cacheTime = -1;
        #endregion
        
        #region 静态接口
        public static AssetTask CreateAsset(string path,Type type)
        {
            AssetTask asset = new AssetTask();
            return asset;
        }
        #endregion
        
        #region 初始化
        public void Init(string path)
        {
            this.path = path;
        }
        
        public void Init(string path,Type type)
        {
            Init(path);
            this.type = type;
        }
        #endregion
        
        #region 资源加载
        protected override void LoadSync()
        {
            if(Main.instance.resource.ResConfig.mode == ResourceMode.Editor)
            {
                string assetPath = "Assets/Res/" + path;
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                SetAsset(asset);
            }
            else
            {
                
            }
        }
        
        protected override IEnumerator LoadAsync()
        {
            if(Main.instance.resource.ResConfig.mode == ResourceMode.Editor)
            {
                string assetPath = "Assets/Res/" + path;
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                SetAsset(asset);
            }
            else
            {
                yield return null;
            }
        }
        #endregion
        
        #region 设置
        public void SetAsset(Object asset)
        {
            if(asset == null)
            {
                CSDebug.LogError(ResourceErrors.ASSET_NOT_EXIT);
                return;
            }

            var gameObject = asset as GameObject;
            SetDone(asset);
        }
        
        public void SetCache(float cacheTime = ResourceManager.ASSET_CACHE_DEFAULT_TIME)
        {
            if(this.cacheTime < cacheTime)
            {
                this.cacheTime = cacheTime;
            }
        }
        #endregion
        
        public override void Release()
        {
            base.Release();
            if(refCount == 1)
            {
                
            }
        }

        public override void Disconnect()
        {
            
        }
    }
}