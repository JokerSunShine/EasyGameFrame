using System;
using System.Text;
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
        public virtual void Awake(){}
        
        public virtual void Start(){}
        
        public virtual void Update(){}
        
        public virtual void LateUpdate(){}
        
        public virtual void FixedUpdate(){}
        
        public virtual void Init(){}

        public virtual void ClearMemory(){}
        
        public virtual void IsEditorMode(){}
        
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
    }
}