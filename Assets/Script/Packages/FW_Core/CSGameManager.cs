using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _3DMath;
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
using Newtonsoft.Json.Bson;
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
using Script.DataStruct.Tree.Heap.MinHeap;
using DataStruct.Tree.Heap.MaxHeap;
using DataStruct.Tree.Heap.BinomialHeap;

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
        GraphAbstract<string> graph = new MatrixGraph<string>(GraphType.UndirectedGraph);
        for(int i = 1;i < 8;i++)
        {
            graph.AddNode("V" + i);
        }
        graph.GraphAddEdge(0,1,2);
        graph.GraphAddEdge(0,2,6);
        graph.GraphAddEdge(1,3,5);
        graph.GraphAddEdge(2,3,8);
        graph.GraphAddEdge(3,5,15);
        graph.GraphAddEdge(3,4,10);
        graph.GraphAddEdge(4,5,6);
        graph.GraphAddEdge(4,6,2);
        graph.GraphAddEdge(5,6,6);
    
        ShortPath<string>.DJSNode[] pathNode = ShortPath<string>.Dijkstra_ShortPath(graph as MatrixGraph<string>,0);
        // ChainQueue<NodeAbstract<string>> nodeQueue = graph.GraphDFS(0);
        // ChainQueue<NodeAbstract<string>> nodeQueue2 = graph.GraphBFS(0);
        Debug.Log(pathNode);
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

