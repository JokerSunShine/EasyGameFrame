using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Packages.FW_Common.Other;
using Script.Packages.FW_Common.Src.UnityEx;
using Script.Packages.FW_Core.Src;
using Script.Packages.FW_Resource.Base;
using UnityEngine;

namespace Script.Packages.FW_Resource.Manager
{
    public static class InstantiateManager
    {
        #region 数据
        /// <summary>
        /// 游戏对象缓存
        /// </summary>
        private static Dictionary<string, Stack<GameObject>> FreeDic = new Dictionary<string, Stack<GameObject>>();

        private static Transform _pool;
        /// <summary>
        /// 对象缓存存放地点
        /// </summary>
        private static Transform Pool
        {
            get
            {
                if(_pool == null)
                {
                    _pool = new GameObject("_ObejctPool").transform;
                }

                return _pool;
            }
        }
        #endregion
        
        #region Intantiate
        public static GameObject Instantiate(string path,Transform parent = null,bool worldPositionStays = true)
        {
            var gameObject = GetCacheGameObject(path, parent, worldPositionStays);
            if (gameObject)
                return gameObject;
            var assetRequest = ResourceManager.LoadAsset(path, typeof(GameObject));
            var request = new RequestInstantiete(assetRequest);
            assetRequest.Dispose();
            request.Instantiate(parent,worldPositionStays);
            return request.gameobject;
        }
        
        public static GameObject Instantiate(string path,Vector3 position,Quaternion quaternion,Transform parent = null,bool worldPositionStays = true)
        {
            var gameObject = GetCacheGameObject(path, position, quaternion, parent);
            if (gameObject)
                return gameObject;
            var assetRequest = ResourceManager.LoadAsset(path, typeof(GameObject));
            var request = new RequestInstantiete(assetRequest);
            assetRequest.Dispose();
            request.Instantiate(position,quaternion,parent);
            return request.gameobject;
        }
        #endregion
        
        #region IntantiateAsync
        public static RequestInstantiete InstantiateAsync(string path,Transform parent = null,bool worldPositionStays = true)
        {
            var gameObject = GetCacheGameObject(path, parent, worldPositionStays);
            if (gameObject)
                return FindRequest(gameObject);
            var assetRequest = ResourceManager.LoadAssetAsync(path, typeof(GameObject));
            var request = new RequestInstantiete(assetRequest);
            Main.instance.StartCoroutine(DoInstantiateAsync(assetRequest, request, parent, worldPositionStays));
            return request;
        }
        
        public static RequestInstantiete InstantiateAsync(string path,Vector3 position,Quaternion quaternion,Transform parent = null,bool worldPositionStays = true)
        {
            var gameObject = GetCacheGameObject(path,position,quaternion,parent);
            if (gameObject)
                return FindRequest(gameObject);
            var assetRequest = ResourceManager.LoadAssetAsync(path, typeof(GameObject));
            var request = new RequestInstantiete(assetRequest);
            Main.instance.StartCoroutine(DoInstantiateAsync(assetRequest, request, position,quaternion,parent));
            return request;
        }
        
        private static IEnumerator DoInstantiateAsync(RequestAsset assetRequest,RequestInstantiete request,Transform parent,bool worldPositionStays)
        {
            yield return assetRequest;
            assetRequest.Dispose();
            request.Instantiate(parent,worldPositionStays);
        }
        
        private static IEnumerator DoInstantiateAsync(RequestAsset assetRequest,RequestInstantiete request,Vector3 position,Quaternion quaternion,Transform parent)
        {
            yield return assetRequest;
            assetRequest.Dispose();
            request.Instantiate(position,quaternion,parent);
        }
        #endregion
        
        #region 获取
        private static GameObject GetCacheGameObject(string path)
        {
            Stack<GameObject> items;
            if(FreeDic.TryGetValue(path,out items) && items.Count > 0)
            {
                GameObject gameObejct = items.Pop();
                Enable(gameObejct);
                return gameObejct;
            }

            return null;
        }
        
        private static GameObject GetCacheGameObject(string path,Transform parent,bool worldPositionStays)
        {
            GameObject gameObject = GetCacheGameObject(path);
            if(gameObject != null)
            {
                Transform transform = gameObject.transform;
                transform.SetParent(parent,worldPositionStays);
            }

            return gameObject;
        }
        
        private static GameObject GetCacheGameObject(string path,Vector3 position,Quaternion quaternion,Transform parent)
        {
            GameObject gameObject = GetCacheGameObject(path);
            if(gameObject != null)
            {
                Transform transform = gameObject.transform;
                transform.SetParent(parent);
                transform.localPosition = position;
                transform.localRotation = quaternion;
            }

            return gameObject;
        }
        
        private static RequestInstantiete FindRequest(GameObject gameObject)
        {
            var list = Temp<DisposeOnDestroy>.List;
            gameObject.GetComponents(list);
            for(int i = 0;i < list.Count;i++)
            {
                var request = list[i].disposable as RequestInstantiete;
                if(request != null)
                {
                    return request;
                }
            }
            return null;
        }
        #endregion
        
        #region 资源设置
        public static void Enable(GameObject gameObject)
        {
            gameObject.hideFlags = HideFlags.None;
            gameObject.SetActive(true);
        }
        
        public static void Disable(GameObject gameObject)
        {
            gameObject.transform.SetParent(Pool);
            gameObject.SetActive(false);
        }
        #endregion
        
        #region 销毁
        public static void Recycle(GameObject gameObject)
        {
            if (!gameObject)
                return;
            RequestInstantiete request = FindRequest(gameObject);
            if(request != null)
            {
                Disable(gameObject);
                Stack<GameObject> items;
                if(!FreeDic.TryGetValue(request.path,out items))
                {
                    items = new Stack<GameObject>();
                    FreeDic[request.path] = items;
                }
                items.Push(gameObject);
            }
            else
            {
                ObjectEX.Destroy(gameObject);
            }
        }
        
        public static void Destroy()
        {
            var dicEnumerator = FreeDic.GetEnumerator();
            while(dicEnumerator.MoveNext())
            {
                var stackEnumerator = dicEnumerator.Current.Value.GetEnumerator();
                while(stackEnumerator.MoveNext())
                {
                    if(stackEnumerator.Current)
                        Object.DestroyImmediate(stackEnumerator.Current);
                }
            }
            FreeDic.Clear();
        }
        #endregion
    }
}