/// <summary>
/// 运行的平台
/// </summary>
public enum OperationPlatform {
    Editor = 1,
    Android = 2,
    IOS = 3,
}

/// <summary>
/// 导出方法类型
/// </summary>
public enum GenMethodType
{
    /// <summary>
    /// 静态方法
    /// </summary>
    Static,
    /// <summary>
    /// 实例方法
    /// </summary>
    Instance,
    /// <summary>
    /// 扩展方法
    /// </summary>
    Extension,
}

/// <summary>
/// 登录的服务器类型(联服....)
/// </summary>
public enum LoginServerType {
    /// <summary>
    /// 游戏服
    /// </summary>
    GameServer = 1,
    /// <summary>
    /// 联服
    /// </summary>
    ShardServer = 2,
}