using System;
using System.Collections.Generic;
using Script.Packages.FW_Core.Src;
using Script.Packages.FW_Resource.Base;
using Script.Packages.FW_Resource.Task;
using UnityEngine;

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
        private static float lastUpdateTime, updateTimeInterval = 1;
        #endregion
        
        #region 加载
        public static RequestAsset Load(string path,Type type,bool async)
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
            UpdateTasks();
        }
        
        public static void UpdateTasks()
        {
            float now = Time.time;
            if (lastUpdateTime + updateTimeInterval > now)
                return;
            lastUpdateTime = now;
            for(int i = taskList.Count;i >= 0;--i)
            {
                var task = taskList[i];
                if(task.refCount == 1)
                {
                    task.Release();
                }
                taskList.RemoveAt(i);
                taskDic.Remove(new TaskKey(task.path, task.type));
            }
        }
        #endregion
        
        #region Destroy
        public static void Destroy()
        {
            for(int i = 0;i <= taskList.Count;i++)
            {
                var task = taskList[i];
                task.Destroy();                
            }
            taskList.Clear();
            taskDic.Clear();
        }
        #endregion
    }
}