﻿using UnityEngine;
public class CSPlatformManagerObject : ManagerObject,I_CSPlatformManager {
    #region 数据
    private static CSPlatformManagerObject instance;
    public static CSPlatformManagerObject Instance {
        get {
            return instance;
        }
    }

    private CSGameManager gameManager;
    public CSGameManager GameManager {
        get {
            return gameManager;
        }
    }

    public RuntimePlatform RunTimePlatform {
        get {
            return Application.platform;
        }
    }

    public UnityEditor.BuildTargetGroup BuildTargetGroup {
        get {
            if (RunTimePlatform == RuntimePlatform.WindowsEditor || RunTimePlatform == RuntimePlatform.WindowsPlayer)
            {
                return UnityEditor.BuildTargetGroup.Standalone;
            }
            else if (RunTimePlatform == RuntimePlatform.Android)
            {
                return UnityEditor.BuildTargetGroup.Android;
            }
            else if (RunTimePlatform == RuntimePlatform.IPhonePlayer)
            {
                return UnityEditor.BuildTargetGroup.iOS;
            }
            return UnityEditor.BuildTargetGroup.Unknown;
        }
    }

    private OperationPlatform operationPlatform;
    public OperationPlatform OperationPlatform {
        get {
            if (operationPlatform != 0)
            {
                return operationPlatform;
            }
            if (RunTimePlatform == RuntimePlatform.WindowsEditor || RunTimePlatform == RuntimePlatform.OSXEditor || RunTimePlatform == RuntimePlatform.LinuxEditor)
            {
                operationPlatform = OperationPlatform.Editor;
            }
            else if (RunTimePlatform == RuntimePlatform.Android)
            {
                operationPlatform = OperationPlatform.Android;
            }
            else if (RunTimePlatform == RuntimePlatform.OSXPlayer)
            {
                operationPlatform = OperationPlatform.IOS;
            };
            return operationPlatform;
        }
    }

    private AbstractPlatformInfo platformInfo;
    public AbstractPlatformInfo PlatformInfo {
        get {
            if(platformInfo == null)
                platformInfo = GetPlatformInfo();
            return platformInfo;
        }
    }
    #endregion

    #region 构造函数
    public CSPlatformManagerObject(CSGameManager gameManager) {
        this.gameManager = gameManager;
        instance = this;
    }
    #endregion

    #region 获取
    private AbstractPlatformInfo GetPlatformInfo()
    {
        return new CSEditorPlatformInfo();
    }
    #endregion
}