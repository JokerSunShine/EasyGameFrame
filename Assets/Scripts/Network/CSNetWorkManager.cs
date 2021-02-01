using System.Threading;

public class CSNetWorkManager : AbstractManager
{
    #region 数据
    private static CSNetWorkManager instance;
    public static CSNetWorkManager Instance
    {
        get
        {
            return instance;
        }
    }
    private CSGameManager gameManager;
    #endregion

    #region 构造函数
    public CSNetWorkManager(CSGameManager gameManager) {
        instance = this;
        this.gameManager = gameManager;
    }
    #endregion

    #region 连接网络
    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="host">主机id</param>
    /// <param name="port">端口id</param>
    /// <param name="loginServerType">登录服务器类型</param>
    public void Connect(string host, string port, LoginServerType loginServerType)
    {
        
    }

    /// <summary>
    /// 启动拆包线程
    /// </summary>
    private void OpenUnpackThread()
    {
        
    }
    #endregion

    #region 请求服务器数据

    #endregion

    #region 接收服务器数据

    #endregion
}
