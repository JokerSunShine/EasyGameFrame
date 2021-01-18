using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGameManager : MonoBehaviour
{
    public List<AbstractManager> managerList = new List<AbstractManager>();
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
        ManagerListAwakeCallBack();
    }

    private void RegisterManager()
    {
        managerList.Add(new CSPlatformManager(this));
        managerList.Add(new XluaMgr(this));
    }

    private void ManagerListAwakeCallBack()
    {
        foreach (AbstractManager manager in managerList)
        {
            manager.Awake();
        }
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
        foreach (AbstractManager manager in managerList)
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
        foreach (AbstractManager manager in managerList)
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
        foreach (AbstractManager manager in managerList)
        {
            manager.Destroy();
        }
    }
    #endregion
}

