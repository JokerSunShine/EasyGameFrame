using CSObjectWrapEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

public class GenBlackList {
    //如果某属性、方法不需要生成，加这个标签
    public class BlackListAttribute : Attribute
    {

    }

    //类 函数 函数参数
    [BlackList]
    public static List<List<string>> AttributeBlackList = new List<List<string>>() {
                new List<string>(){"UnityEngine.WWW", "movie"},
                new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
                new List<string>(){"UnityEngine.Application", "ExternalEval"},
                new List<string>(){"UnityEngine.GameObject", "networkView"},
                new List<string>(){"UnityEngine.Component", "networkView"},
                new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
                //和blackTypeToMedthods重复配置一份（是不是可以删除，尚未验证）
                new List<string>(){ "UnityEngine.Input", "IsJoystickPreconfigured" },
                new List<string>(){ "UnityEngine.Texture", "imageContentsHash" },
                new List<string>(){ "System.Environment", "SystemDirectory"},
                new List<string>(){ "System.IO.Directory", "CreateDirectory", "System.String", "System.Security.AccessControl.DirectorySecurity" },
                new List<string>(){ "System.Reflection.Module", "GetSignerCertificate"},
                new List<string>(){ "System.IO.Directory", "GetAccessControl",},
                new List<string>(){ "System.IO.Directory", "SetAccessControl", },
                new List<string>(){"System.Type", "IsSZArray" },
                new List<string>(){ "UnityEngine.MeshRenderer", "receiveGI" },
    };

    //类 函数名（不管该函数有多少个重载，都不生成）
    [BlackList]
    public static Dictionary<string, List<string>> MethodBlackList = new Dictionary<string, List<string>>()
        {
            {"Top_CSGame",new List<string>(){"OnDrawGizmos","_IsShowSceneMesh"}},
            { "UnityEngine.Input", new List<string>(){"IsJoystickPreconfigured" } },
            { "UnityEngine.Texture", new List<string>(){"imageContentsHash" } },
            { "System.Enum", new List<string>(){ "HasFlag" } },
            { "System.Environment", new List<string>(){ "SystemDirectory" } },
            { "System.IO.DirectoryInfo", new List<string>(){"GetAccessControl","SetAccessControl"} },
            { "System.Reflection.Module", new List<string>(){ "GetSignerCertificate" } },
            { "System.IO.Directory", new List<string>(){ "GetAccessControl", "SetAccessControl", } },
            { "System.Type",new List<string>(){"IsSZArray"}},
            { "UnityEngine.MeshRenderer",new List<string>(){"receiveGI"}}
    };

    //类（整个类都不生成）
    [BlackList]
    public static List<string> ClassBlackList = new List<string>()
    {
        "UIDrawCall",
        "UIGeometry",
        "ProtoOptimize",
        "CSAtlasCollect",
        "CSDebug",
        "CSExceptionCatch",
        "FPS",
        "ToolPhoneStateGet",
        "UnZipClass",
        "ZipClass",
        "CSHandRoot",
        "CSEffectAnimationOverDisable",
        "SDebug",
        "NGUIDebug",
        "XmlUtil",
        //"Net",//将消息请求丢去掉热更，发送消息是触发型的，减少代码量
        "CSToolsOpenPanel",
        "TXVoiceInterfaceClient",
        "System.String",
        "System.Environment.SpecialFolderOption",
        "System.Environment.SpecialFolder",
    };

    public static bool isMemberInBlackList(MemberInfo mb)
    {
        if (mb.IsDefined(typeof(BlackListAttribute), false))
            return true;
        if (Generator.BlackList == null) return false;
        foreach (var exclude in Generator.BlackList)
        {
            if (mb.DeclaringType.FullName == exclude[0] && mb.Name == exclude[1])
            {
                return true;
            }
        }
        return false;
    }
}
