using System;
using Script.Packages.FW_Core.Src;
using Script.Packages.FW_Resource.Base;
using Script.Packages.FW_Resource.Manager;
using UnityEngine;

namespace Script.Packages.FW_Resource.ResourceLoad
{
    [Serializable]
    #region 枚举
    public enum ResourceMode
    {
        Editor = 0,
        Package = 1,
        HotUpdate = 2,
    }
    #endregion
    
    [Serializable]
    public class ResConfig
    {
        public ResourceMode mode;
    }
    
    #region 构造
    public class ResourceAsset : AbstractResource
    {
        #region 数据
        [SerializeField]
        public ResConfig resConfig = new ResConfig();
        public override ResConfig ResConfig
        {
            get => resConfig;
            set => resConfig = value;
        }
        #endregion

        #region 生命周期
        public override void Awake()
        {
            
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override void Init()
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }

        public override void ClearMemory()
        {
        }
        
        public override void Destroy()
        {
            
        }
        #endregion

        
        #region File
        public override  bool FileExists(string path)
        {
            return false;
        }

        public override  void FixedUpdate()
        {
        }

        public override  void LateUpdate()
        {
        }

        public override  byte[] LoadFile(string path)
        {
            return null;
        }

        public override  void SaveFile(string path, byte[] bytes)
        {
        }

        public override  void SaveFile(string path, string text)
        {
        }
        #endregion
  
        #region Asset
        public override RequestAsset LoadAsset(string path, Type type)
        {
            return AssetManager.Load(path, type, false);
        }

        public override RequestAsset LoadAssetAsync(string path, Type type)
        {
            return AssetManager.Load(path, type, true);
        }
        #endregion
        
        #region Instatiate
        
        #endregion
    }
    #endregion
}