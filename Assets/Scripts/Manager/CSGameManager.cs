using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGameManager : MonoBehaviour
{
    public List<AbstractManager> managerList = new List<AbstractManager>();
    #region 初始化
    private void Awake()
    {
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
        ManagerListUpdateCallBack();
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

