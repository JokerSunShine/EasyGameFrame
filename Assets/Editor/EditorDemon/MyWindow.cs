using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEditor;

public class MyWindow : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    private static MyWindow m_MyWindow;
    private Vector2 scrollPosition = Vector2.zero;

    private float hSbarValue;

    private List<string> pathList = new List<string>();
    private string inputPath;
    private Rect hintPanelRect = new Rect(20,20,120,50);

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        m_MyWindow = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
        m_MyWindow.Show();
        m_MyWindow.titleContent.image = EditorGUIUtility.IconContent("UnityLogo").image;
        
    }
    
    void OnGUI()
    {
        GUILayout.Label("手动路径修改列表");
        GUILayout.BeginHorizontal();
        inputPath = GUILayout.TextField("输入内容");
        if(GUILayout.Button("加入打开列表"))
        {
            bool addResult = AddPathList(inputPath);
            if(addResult == false)
            {
                
            }
        }
        GUILayout.EndHorizontal();
    }
    
    private bool AddPathList(string path)
    {
        if(Directory.Exists(path) == false || Directory.Exists(path) == false)
        {
            CSDebug.Log("不存在目录或文件夹\r\n" + path);
            return false;
        }
        pathList.Add(path);
        return true;
    }
    
    
    private void OpenHintPanel(string content,GUI.WindowFunction func)
    {
        GUILayout.Window(0,hintPanelRect,_=>
        {
            if(GUILayout.Button("确认"))
            {
                
            }
        },content);
    }
}