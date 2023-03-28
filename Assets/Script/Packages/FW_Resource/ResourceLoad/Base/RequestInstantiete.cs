using Packages.FW_Common.Other;
using Script.Packages.FW_Resource.Task;
using UnityEngine;

namespace Script.Packages.FW_Resource.Base
{
    public class RequestInstantiete:RequestAsset
    {
        #region 数据
        public override bool isDone
        {
            get
            {
                return base.isDone && (isError || gameobject);
            }
        }

        public GameObject gameobject;
        #endregion
        
        #region 构造
        public RequestInstantiete(RequestAsset request):base(request.task){}
        #endregion
    
        #region 实例化
        public void Instantiate(Transform parent,bool worldPositionStays)
        {
            if(isError)
                return;
            var assetTask = task as AssetTask;
            gameobject = Object.Instantiate(assetTask.asset as GameObject);
            gameobject.name = assetTask.asset.name;
            DisposeOnDestroy.Add(gameobject,this);
        }
        
        public void Instantiate(Vector3 position,Quaternion quaternion,Transform parent)
        {
            if (isError)
                return;
            var assetTask = task as AssetTask;
            gameobject = Object.Instantiate(assetTask.asset as GameObject,position,quaternion,parent);
            gameobject.name = assetTask.asset.name;
            DisposeOnDestroy.Add(gameobject,this);
        }
        
        #endregion
    }
}