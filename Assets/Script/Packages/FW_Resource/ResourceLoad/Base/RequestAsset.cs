using System;

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
        }
        #endregion
        
        public string path
        {
            get { return task != null ? task.path : string.Empty; }
        }
    }
}