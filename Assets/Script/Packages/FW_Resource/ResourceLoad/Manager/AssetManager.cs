using System;
using System.Collections.Generic;
using Script.Packages.FW_Core.Src;
using Script.Packages.FW_Resource.Base;
using Script.Packages.FW_Resource.Task;

namespace Script.Packages.FW_Resource.Manager
{
    struct TaskKey : IEquatable<TaskKey>
    {
        public string path;
        public Type type;
            
        public TaskKey(string path,Type type)
        {
            this.path = path;
            this.type = type;
        }
            
        public bool Equals(TaskKey key)
        {
            return path == key.path && type == key.type;
        }
            
        public override int GetHashCode()
        {
            return path.GetHashCode();
        }
    }   
    
    public static class AssetManager
    {
        #region 数据
        public static List<AssetTask> taskList = new List<AssetTask>();
        private static Dictionary<TaskKey,AssetTask> taskDic = new Dictionary<TaskKey, AssetTask>();
        #endregion
        
        #region 加载
        public static RequestAsset Load(string path,Type type,bool async,float cacheTime = ResourceManager.ASSET_CACHE_DEFAULT_TIME)
        {
            AssetTask task = FindTask(path, type);
            if(task == null)
            {
                task = AssetTask.CreateAsset(path, type);
                taskList.Add(task);
                taskDic[new TaskKey(path, type)] = task;
                task.Load(async);
            }
            else
            {
                if(!async)
                {
                    while (!task.IsDone) ;
                }
            }
            
            return new RequestAsset(task);
        }
        #endregion
        
        #region 获取
        public static AssetTask FindTask(string path,Type type)
        {
            if (taskDic.TryGetValue(new TaskKey(path, type), out AssetTask task))
                return task;
            return null;
        }
        #endregion
        
        #region Update
        public static void Update()
        {
            
        }
        #endregion
    }
}