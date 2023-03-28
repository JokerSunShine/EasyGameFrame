using System;
using System.Text;
using Script.Packages.FW_Resource.Base;
using Script.Packages.FW_Resource.ResourceLoad;
using UnityEngine;

namespace Script.Packages.FW_Core.Src
{
    public abstract class AbstractResource : ScriptableObject,IMonohaviour
    {
        public abstract ResConfig ResConfig
        {
            get;
            set;
        }
        #region 生命周期
        public virtual void Awake(){}
        
        public virtual void Start(){}
        
        public virtual void Update(){}
        
        public virtual void LateUpdate(){}
        
        public virtual void FixedUpdate(){}
        
        public virtual void Init(){}

        public virtual void ClearMemory(){}
        
        public virtual void IsEditorMode(){}
        
        public virtual void Destroy(){}
        #endregion

        #region File
        public virtual bool FileExists(string path)
        {
            return false;
        }

        public virtual long GetFileSize(string path)
        {
            return 0;
        }
        
        public virtual byte[] LoadFile(string path)
        {
            return null;
        }
        
        public virtual string LoadFileText(string path,Encoding encoding = null)
        {
            return "";
        }
        
        public virtual void SaveFile(string path,byte[] bytes){}
        
        public virtual void SaveFile(string path,string text){}
        #endregion
        
        #region Bundle
        #endregion
        
        #region Asset
        public virtual RequestAsset LoadAsset(string path,Type type)
        {
            return null;
        }

        public virtual RequestAsset LoadAssetAsync(string path,Type type)
        {
            return null;
        }
        #endregion
        
        #region Instatiate
        public virtual GameObject Instantiate(string path,Transform parent = null,bool worldPositionStays = true)
        {
            return null;
        }

        public virtual GameObject Instantiate(string path,Vector3 position,Quaternion quaternion,Transform parent = null,bool worldPositionStays = true)
        {
            return null;
        }

        public virtual RequestInstantiete Intantiate(string path,Transform parent = null,bool worldPositionStays = true)
        {
            return null;
        }

        public virtual RequestInstantiete Intantiate(string path,Vector3 position,Quaternion quaternion,Transform parent = null,bool worldPositionStays = true)
        {
            return null;
        }
        
        public virtual void Recycle(GameObject gameObject){}
        #endregion
    }
}