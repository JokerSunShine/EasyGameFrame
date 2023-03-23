using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _3DMath;
using Algorithm.GraphArithmetic;
using Algorithm.MiniSpanTree.MiniSpanTree;
using Common.DataStruct.Queue.ChainQueue;
using Common.OneWayChainList;
// using Common.BothWayChainList;
using Common.SequenceList;
using DataStruct.Graph.ALGraph;
using DataStruct.Graph.Base;
using DataStruct.Graph.EGGraph;
using DataStruct.Graph.MatrixGraph;
using DataStruct.Tree.BTree.Base;
using DataStruct.Tree.BTree.BPlusTree;
using DataStruct.Tree.BTree.BTree;
using Instance.LogAOP;
using UnityEngine;
// using DataStruct.Tree.BinaryTree;
// using DataStruct.Tree.BinaryTree.AVLTree;
// using DataStruct.Tree.BinaryTree.BinarySearchTree;
// using DataStruct.Tree.BinaryTree.FullBinaryTree;
// using DataStruct.Tree.BinaryTree.PerfectBinaryTree;
// using DataStruct.Tree.BinaryTree.RedBlackTree;
// using DataStruct.Tree.BTree.BTree;
// using TreeEditor;
// using UnityEditor.Experimental.GraphView;
// using DataStruct.Tree.BTree.Base;

// using Vector3 = _3DMath.Vector3;
using Algorithm.ShortPath;
using Algorithm.Sort;
using Algorithm.StringMatch;
using Script.DataStruct.Tree.Heap.MinHeap;
using DataStruct.Tree.Heap.MaxHeap;
using DataStruct.Tree.Heap.BinomialHeap;
using Script.Algorithm.BagAlgorithm;
using Script.Packages.FW_Core.Src;
using Script.Packages.FW_Resource.ResourceLoad;
using Script.Packages.FW_Resource.Task;

public class Main : MonoBehaviour
{
    public static Main instance;
    public Dictionary<string,ManagerObject> managerDic = new Dictionary<string,ManagerObject>();
    public bool OpenLog = true;
    public AbstractResource resource;
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
        instance = this;
        InitOther();
        RegisterManager();
        RegiseterInterfaceSingleton();
        ManagerListAwakeCallBack();
        AOPTest();
        BagItem[] bagItems = new BagItem[5]
        {
            new BagItem(1, 2, 3),
            new BagItem(2, 6, 5),
            new BagItem(3, 5, 10),
            new BagItem(4, 4, 7),
            new BagItem(5, 5, 11),
        };
        BagAlgorithm bagAlgorithm = new BagAlgorithm(bagItems,10);
        List<BagItem> insertBagItems = new List<BagItem>();
        int value = bagAlgorithm.GetMaxValueByCompleteBag(5, 10, insertBagItems);
    }
    
    public int CompareFunc(int i,int j)
    {
        return i - j;
    }
    
    private int IntCompare(int num1,int num2)
    {
        if(num1 > num2)
        {
            return 1;
        } 
        else if(num1 < num2)
        {
            return -1;
        }
        else
        {
            return 0;
        }
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
        // AOPTest a = new AOPTest();
        // a.TestMethod(1,2);
        // int c = 3,d = 4;
        // Instance.LogAOP.AOPTest.Before(ref c,ref d);
        // Instance.LogAOP.AOPTest.After(5);
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

