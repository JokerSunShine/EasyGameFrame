using UnityEngine;
using System.Collections;
using Script.Packages.FW_Resource.Base;

namespace Script.Packages.FW_Resource.Task
{
    public class AssetTask : TaskBase
    {
        #region 数据
        public Object asset
        {
            get
            {
                return res as Object;
            }
        }
        #endregion
        
        protected override IEnumerator LoadAsync()
        {
            yield return null;
        }
    }
}