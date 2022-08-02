using System.Collections;
using System.Collections.Generic;
using Instance.LogAOP;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class CSGameManager : MonoBehaviour
{
    public Dictionary<string,ManagerObject> managerDic = new Dictionary<string,ManagerObject>();
    public bool OpenLog = true;
    /// <summary>
    /// 刷新频率
    /// </summary>
    public int updateFrequency = 5;
    /// <summary>
    /// 当前刷新
    /// </summary>
    private int curFrequency = 0;
    #region 初始化
    private void Awake()
    {
        InitOther();
        RegisterManager();
        RegiseterInterfaceSingleton();
        ManagerListAwakeCallBack();
        AOPTest();
    }

    private void RegisterManager()
    {
        managerDic["CSPlatformManagerObject"] = new CSPlatformManagerObject(this);
        managerDic["XluaMgr"] = new XluaMgr(this);
        managerDic["CSNetWorkManager"] = new CSNetWorkManager(this);
    }
    
    private void RegiseterInterfaceSingleton()
    {
        InterfaceSingleton.IPlatformManager = GetManagerByName<CSPlatformManagerObject>("CSPlatformManagerObject");
    }

    private void ManagerListAwakeCallBack()
    {
        foreach (ManagerObject manager in managerDic.Values)
        {
            manager.Awake();
        }
    }
    
    /// <summary>
    /// 方法调用拦截
    /// </summary>
    private void AOPTest()
    {
        AOPTest a = new AOPTest();
        a.TestMethod(1,2);
        int c = 3,d = 4;
        Instance.LogAOP.AOPTest.Before(ref c,ref d);
        Instance.LogAOP.AOPTest.After(5);
    }

    private void InitOther()
    {
        CSDebug.LogSwitch = OpenLog;
    }

    private void Start()
    {
        ManagerListStartCallBack();
    }

    private void ManagerListStartCallBack()
    {
        foreach (ManagerObject manager in managerDic.Values)
        {
            manager.Start();
        }
    }
    #endregion

    #region Update
    private void Update()
    {
        if (curFrequency >= updateFrequency)
        {
            ManagerListUpdateCallBack();
            curFrequency = 0;
        }
        curFrequency++;
    }

    private void ManagerListUpdateCallBack()
    {
        foreach (ManagerObject manager in managerDic.Values)
        {
            manager.Update();
        }
    }
    #endregion

    #region Destroy
    private void OnDestroy()
    {
        ManagerListDestroyCallBack();
    }

    private void ManagerListDestroyCallBack()
    {
        foreach (ManagerObject manager in managerDic.Values)
        {
            manager.Destroy();
        }
    }
    #endregion
    
    #region 获取
    /// <summary>
    /// 通过管理类名字获取管理类实例
    /// </summary>
    /// <param name="managerName"></param>
    /// <returns></returns>
    public T GetManagerByName<T>(string managerName) where T:ManagerObject
    {
        ManagerObject manager;
        managerDic.TryGetValue(managerName, out manager);
        T curManager = manager == null ? null: manager as T;
        return curManager;
    }
    #endregion
}

