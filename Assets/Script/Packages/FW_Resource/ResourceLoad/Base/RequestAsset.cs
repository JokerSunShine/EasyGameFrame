using System;
using Object = UnityEngine.Object;

namespace Script.Packages.FW_Resource.Base
{
    public class RequestAsset
    {
        #region 数据
        private TaskBase task;
        private Action mCallBack;
        #endregion
        
        #region 构造
        public RequestAsset(TaskBase task)
        {
            this.task = task;
            task.Retain();
        }
        #endregion
        
        public string path
        {
            get { return task != null ? task.path : string.Empty; }
        }
        
        public float progress
        {
            get { return task != null ? task.progress : 0; }
        }
        
        public virtual bool isLoading
        {
            get { return task != null ? task.IsLoading : false; }
        }
        
        public virtual bool isDone
        {
            get { return task != null ? task.IsDone : true; }
        }
        
        public bool isError
        {
            get { return task != null ? task.IsError : true; }
        }
        
        public string error
        {
            get { return task != null ? task.Error : "request error"; }
        }
        
        public bool disposed
        {
            get { return task == null; }
        }
        
        public object res
        {
            get { return task != null ? task.res : null; }
        }
        
        public Object asset
        {
            get { return res as Object; }
        }
        
        public virtual bool MoveNext()
        {
            return !isDone;
        }
        
        public void Dispose()
        {
            if(task != null)
            {
                if(mCallBack != null)
                {
                    if (!task.IsDone)
                        task.callBack -= mCallBack;
                    mCallBack = null;
                }
                task.Release();
                task = null;
            }

            mCallBack = null;
        }
        
        public void AddAction(Action callBack)
        {
            if (task == null)
                return;
            if (task.IsDone)
                callBack();
            else
            {
                mCallBack += callBack;
                task.callBack += callBack;
            }
        }
        
        public void Disconnect()
        {
            task?.Disconnect();
            if(disposed)
            {
                Dispose();
            }
        }
    }
}