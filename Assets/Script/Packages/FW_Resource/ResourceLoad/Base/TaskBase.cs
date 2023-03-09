using System;
using System.Collections;
using UnityEngine;

namespace Script.Packages.FW_Resource.Base
{
    public abstract class TaskBase
    {
        #region 数据
        public string path;
        public object res;
        public Action callBack;
        
        public virtual float progress
        {
            get
            {
                return isDone ? 1 : 0;
            }
        }

        protected bool isDone = false;
        public bool IsDone
        {
            get
            {
                return isDone;
            }
        }

        protected string error;
        public virtual string Error
        {
            get
            {
                return error;
            }
        }
        
        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(error);
            }
        }

        protected Coroutine coroutine;
        public bool IsLoading
        {
            get
            {
                return coroutine != null;
            }
        }
        #endregion
        
        #region 初始化
        public void Init(string path)
        {
            this.path = path;
        }
        #endregion
        
        #region 功能
        public virtual void SetDone(object res)
        {
            isDone = true;
            this.res = res;
            DoCallBack();
        }
        
        public void DoCallBack()
        {
            if(callBack != null)
            {
                callBack();
                callBack = null;
            }
        }
        
        public void SetError(string error)
        {
            isDone = true;
            this.error = error;
            DoCallBack();
        }
        #endregion
        
        #region 加载
        public void Load(bool async)
        {
            StopLoad();
            if (async)
                CSGameManager.instance.StartCoroutine(DoLoadAsync());
            else
                LoadSync();
        }
        
        IEnumerator DoLoadAsync()
        {
            yield return LoadAsync();
            coroutine = null;
        }

        protected abstract IEnumerator LoadAsync();
        
        protected virtual void LoadSync()
        {
            var e = DoLoadAsync();
            while(e.MoveNext())
            {
                System.Threading.Thread.Sleep(1);
            }
        }
        public void StopLoad()
        {
            if(coroutine != null)
            {
                CSGameManager.instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
        #endregion
        
        #region 删除
        public virtual void Destroy()
        {
            res = null;
            callBack = null;
            StopLoad();
        }
        #endregion
    }
}