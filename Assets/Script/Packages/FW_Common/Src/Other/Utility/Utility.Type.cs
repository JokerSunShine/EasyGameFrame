using System;
using System.Collections.Generic;
using System.Reflection;
using XLua;

namespace Framework
{
    public static partial class Utility
    {
        public static class Type
        {
            private static string[] IgnoreFileNameChars = new string[] { "+",".", "`","&","[","]","," };
            private static string[] IgnoreTagNameChars = new string[] { "+", ".", "`", "&", "[", "]", "," };

            private static readonly string[] AssemblyNames =
            {
                "Assembly-CSharp"
            };
            
            private static readonly string[] EditorAssemblyNames =
            {
                "Assembly-CSharp-Editor"
            };
            
            /// <summary>
            /// 获取当前所有程序域下所有程序集
            /// </summary>
            public static List<System.Type> GetAllTypes()
            {
                List<System.Type> typeList = new List<System.Type>();
                try
                {
                    List<System.Reflection.Assembly> assemblyList = new List<System.Reflection.Assembly>();
                    var assemblies = Assembly.GetAssemblies();
                    assemblyList.AddRange(assemblies);
                    for (int i = 0; i < assemblyList.Count; i++)
                    {
                        CSDebug.Log("dll = " + assemblyList[i].Location);

                        System.Type[] types = assemblyList[i].GetTypes();
                        foreach (var type in types)
                        {
                            if ((type.Namespace != null && type.Namespace.StartsWith("UnityEditor")))
                            {
                                continue;
                            }
                            if (IsNameCurSpace(type, "UnityEditor"))
                            {
                                continue;
                            }
                            typeList.Add(type);
                        }
                    }
                }
                catch(System.Exception ex) {

                }
                return typeList;
            }

            /// <summary>
            /// 类是否继承或包含指定的命名空间
            /// </summary>
            /// <param name="type"></param>
            /// <param name="str"></param>
            /// <returns></returns>
            public static bool IsNameCurSpace(System.Type type,string str)
            {
                if (type == null) return false;
                if (type.Namespace == str) return true;
                type = type.BaseType;
                return IsNameCurSpace(type,str);
            }

            /// <summary>
            /// 获取类型的文件名
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static string GetTypeFileName(System.Type type)
            {
                return GetTypeFileName(type.ToString());
            }

            /// <summary>
            /// 获取类型名字文件名
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            public static string GetTypeFileName(string typeName)
            {
                if (IgnoreFileNameChars == null || IgnoreFileNameChars.Length == 0)
                {
                    return typeName;
                }
                string curTypeName = typeName;
                foreach (string ignoreChar in IgnoreFileNameChars)
                {
                    curTypeName = curTypeName.Replace(ignoreChar,"");
                }
                return curTypeName;
            }
            
            /// <summary>
            /// 获取类型的标签名
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            public static string GetTypeTagName(string typeName)
            {
                if (IgnoreTagNameChars == null || IgnoreTagNameChars.Length == 0)
                {
                    return typeName;
                }
                foreach (string ignoreChar in IgnoreTagNameChars)
                {
                    typeName = typeName.Replace(ignoreChar, "");
                }
                return typeName;
            }
            
            public static string[] GetTypeNames(System.Type typeBase)
            {
                return GetTypeNames(typeBase, AssemblyNames);
            }
            
            public static string[] GetEditorTypeNames(System.Type typeBase)
            {
                return GetTypeNames(typeBase, EditorAssemblyNames);
            }
            
            private static string[] GetTypeNames(System.Type typeBase,string[] assemblyNames)
            {
                List<string> typeNames = new List<string>();
                foreach(string assemblyName in assemblyNames)
                {
                    System.Reflection.Assembly assembly;
                    try
                    {
                        assembly = System.Reflection.Assembly.Load(assemblyName);
                    }
                    catch
                    {
                        continue;
                    }
                    if(assembly == null)
                    {
                        continue;
                    }

                    System.Type[] types = assembly.GetTypes();
                    foreach(System.Type type in types)
                    {
                        if(type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                        {
                            typeNames.Add(type.FullName);
                        }
                    }
                }
                typeNames.Sort();
                return typeNames.ToArray();
            }
        }
        
        /// <summary>
        /// 获取类型的标签名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeTagName(this System.Type type)
        {
            return Type.GetTypeTagName(type.ToString());
        }

        /// <summary>
        /// 是否是指定的类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="curType"></param>
        /// <returns></returns>
        public static bool IsDefined(this System.Type type, System.Type curType)
        {
            if (type == null)
            {
                return false;
            }
            return type.IsDefined(curType,false);
        }
    }
}
