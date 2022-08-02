using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Common.OneWayChainList;
using Common.SequenceList;
using Common.TrieSearch;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Object = System.Object;
using Common.BothWayLoopChainList;
using Common.BothWayChainList;
using Common.OneWayLoopChainList;
using Common.DataStruct.Stack.SequenceStack;
using Common.DataStruct.Stack.ChianStack;
using Common.DataStruct.Queue.SequenceQueue;
using Common.DataStruct.Queue.ChainQueue;
using Common.DataStruct.Queue.BlockingQueue;

public class test : MonoBehaviour
{
    private string[] words = new string[]
    {
        "测试",
        "测试A",
        "测试B",
        "测试C",
        "测试D",
        "华为",
        "苹果",
    };

    private string[] words2 = new[]
    {
        "wordA",
        "wordB",
        "wordC",
        "wordD",
        "wordE",
        "wordF",
        "wordG"
    };
    BlockingQueue<string> stack = new BlockingQueue<string>(10);
    void Start()
    {
        // SequenceQueue<string> queue = new SequenceQueue<string>(words);
        // SequenceQueue<string> queue2 = new SequenceQueue<string>(words2);
        // queue.Dequeue();
        // queue.Dequeue();
        // queue.Enqueue("安卓");
        // queue.Enqueue("安卓2");
        // queue.Enqueue("安卓3");



        // string popValue = stack.Dequeue();
        // stack.Enqueue("测试");
        // int count = stack.Count;
        // for(int i  = 0;i < count;i++)
        // {
        //     stack.Dequeue();
        // }
        // Debug.Log(stack.Count);
        
        Thread send = new Thread(SendMessage);
        send.IsBackground = false;
        Thread receive = new Thread(ReceiveMessage);
        receive.IsBackground = false;
        send.Start();
        receive.Start();
        int index = 0;
    }
    
    public void SendMessage()
    {
        // int index = 0;
        // while(true)
        // {
        //     Thread.Sleep(TimeSpan.FromSeconds(0.01f));
        //     string Value = words[index++];
        //     stack.Enqueue(Value);
        //     if(index >= words.Length)
        //     {
        //         index = 0;
        //     }
        // }
    }
    
    public void ReceiveMessage()
    {
        string str = string.Empty;
        while(true)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.5f));
            stack.TryDequeue(out str);
            Debug.Log("------提取数据" + str);
        }
    }

    // Update is called once per frame
    int index = 0;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            string Value = words[index++];
            stack.Enqueue(Value);
            CSDebug.Log("+++++++添加数据" + Value);
            if(index >= words.Length)
            {
                index = 0;
            }
        }
    }
}