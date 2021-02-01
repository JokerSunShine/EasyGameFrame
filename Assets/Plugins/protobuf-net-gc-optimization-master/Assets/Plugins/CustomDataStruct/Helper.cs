using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CustomDataStruct
{
    public static class Helper
    {
        //#pragma warning disable 0414
        //        static CustomDataStructHelper helper =
        //            (new GameObject("CustomDataStructHelper")).AddComponent<CustomDataStructHelper>();
        //#pragma warning restore 0414

        public static void Startup()
        {
        }

        public static void Cleanup()
        {
#if UNITY_EDITOR
            Debug.Log("CustomDataStruct Cleanup!");
#endif
            BetterDelegate.Cleanup();
            BetterStringBuilder.Cleanup();
            ValueObject.Cleanup();
            ObjPoolBase.Cleanup();
#if UNITY_EDITOR
            MemoryLeakDetecter.Cleanup();
#endif
        }

#if UNITY_EDITOR
        public static void ClearDetecterUsingData()
        {
            List<MemoryLeakDetecter> deteters = MemoryLeakDetecter.detecters;
            for (int i = 0; i < deteters.Count; i++)
            {
                deteters[i].ClearUsingData();
            }
        }
#endif

        public static string HandleTypeFullName(string name)
        {
            string[] list = name.Split(',');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Length; i++)
            {
                string cur = list[i];
                if (!cur.Contains("Assembly") &&
                    !cur.Contains("mscorlib") &&
                    !cur.Contains("Version") &&
                    !cur.Contains("Culture")
                    )
                {
                    if (cur.Contains("PublicKeyToken"))
                    {
                        int startIndex = cur.IndexOf(']');
                        if (startIndex >= 0)
                        {
                            sb.Append(cur.Substring(startIndex));
                        }
                    }
                    else
                    {
                        sb.Append(cur);
                    }
                }
            }
            return sb.ToString();
        }
    }

    internal sealed class CustomDataStructHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        private const float LOG_INTERVAL = 1.0f;
        public bool debug = true;
        public bool log = false;
        private float nextLogTime = 0f;
#endif
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
            nextLogTime = Time.realtimeSinceStartup + LOG_INTERVAL;
#endif
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (debug)
            {
                List<MemoryLeakDetecter> deteters = MemoryLeakDetecter.detecters;
                for (int i = 0; i < deteters.Count; i++)
                {
                    deteters[i].DetectMemoryLeaks();
                }
            }

            log = debug ? log : debug;
            if (log && nextLogTime < Time.realtimeSinceStartup)
            {
                StringBuilder sb = new StringBuilder();
                nextLogTime = Time.realtimeSinceStartup + LOG_INTERVAL;
                List<MemoryLeakDetecter> deteters = MemoryLeakDetecter.detecters;
                for (int i = 0; i < deteters.Count; i++)
                {
                    sb.AppendLine(deteters[i].ToLogString());
                }
                Debug.Log(sb.ToString());
            }
        }
#endif

        public void OnDisable()
        {
            Helper.Cleanup();
        }
    }
}