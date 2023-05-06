using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class EditorCoroutineRunner {
    #region 数据结构
    private class EditorCoroutine : IEnumerator
    {
        #region 数据
        private Stack<IEnumerator> coroutineList;
        public object Current
        {
            get
            {
                return coroutineList.Peek().Current;
            }
        }
        #endregion


        #region 构造函数
        public EditorCoroutine(IEnumerator iEnumerator)
        {
            coroutineList = new Stack<IEnumerator>();
            coroutineList.Push(iEnumerator);
        }

        public EditorCoroutine(List<IEnumerator> iEnumeratorList)
        {
            coroutineList = new Stack<IEnumerator>();
            foreach (var coroutine in iEnumeratorList)
            {
                coroutineList.Push(coroutine);
            }
        }
        #endregion

        public bool MoveNext()
        {
            IEnumerator i = coroutineList.Peek();
            if (i.MoveNext())
            {
                object nextObj = i.Current;
                if (nextObj != null && nextObj is IEnumerator)
                {
                    coroutineList.Push((IEnumerator)nextObj);
                }
                return true;
            }
            else {
                if (coroutineList.Count > 1)
                {
                    coroutineList.Pop();
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            throw new System.NotSupportedException("This Operation Is Not Supported.");
        }

        public bool Find(IEnumerator coroutine)
        {
            return coroutineList.Contains(coroutine);
        }
    }
    #endregion

    #region 数据
    /// <summary>
    /// 待启动的携程列表
    /// </summary>
    private static List<IEnumerator> DelayPushCoroutineList = new List<IEnumerator>();
    /// <summary>
    /// 正在执行的携程列表
    /// </summary>
    private static List<EditorCoroutine> CoroutineList = new List<EditorCoroutine>();
    #endregion

    #region 接口
    public static IEnumerator StartEditorCoroutine(IEnumerator iEnumerator)
    {
        if(CoroutineList.Count == 0)
        {
            EditorApplication.update += Update;
        }
        DelayPushCoroutineList.Add(iEnumerator);
        return iEnumerator;
    }
    #endregion

    #region 内部功能
    private static void Update()
    {
        //移除没有数据的携程对象
        CoroutineList.RemoveAll(coroutinue => {
            return coroutinue.MoveNext() == false;
        });
        if (DelayPushCoroutineList.Count > 0)
        {
            for (int i = DelayPushCoroutineList.Count - 1;i == 0;i--)
            {
                if (Find(DelayPushCoroutineList[i]) == true)
                {
                    DelayPushCoroutineList.RemoveAt(i);
                }
            }
            CoroutineList.Add(new EditorCoroutine(DelayPushCoroutineList));
        }
        DelayPushCoroutineList.Clear();

        if (CoroutineList.Count == 0)
        {
            EditorApplication.update -= Update;
        }
    }

    /// <summary>
    /// 是否有数据
    /// </summary>
    /// <returns></returns>
    private static bool Find(IEnumerator coroutine)
    {
        foreach (var coroutineMgr in CoroutineList)
        {
            if (coroutineMgr.Find(coroutine))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}