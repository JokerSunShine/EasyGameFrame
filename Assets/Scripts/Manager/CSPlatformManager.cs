using UnityEngine;
public class CSPlatformManager:AbstractManager{
    #region 数据
    private static CSPlatformManager instance;
    public static CSPlatformManager Instance {
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

    public RuntimePlatform runTimePlatform {
        get {
            return Application.platform;
        }
    }

    private OperationPlatform operationPlatform;
    public OperationPlatform OperationPlatform {
        get {
            if (operationPlatform != 0)
            {
                return operationPlatform;
            }
            if (runTimePlatform == RuntimePlatform.WindowsEditor || runTimePlatform == RuntimePlatform.OSXEditor || runTimePlatform == RuntimePlatform.LinuxEditor)
            {
                operationPlatform = OperationPlatform.Editor;
            }
            else if (runTimePlatform == RuntimePlatform.Android)
            {
                operationPlatform = OperationPlatform.Android;
            }
            else if (runTimePlatform == RuntimePlatform.OSXPlayer)
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
    public CSPlatformManager(CSGameManager gameManager) {
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