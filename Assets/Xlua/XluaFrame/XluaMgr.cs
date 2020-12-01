using System;
using UnityEngine;
using XLua;
public class XluaMgr:AbstractManager
{
    #region 委托类型
    [CSharpCallLua]
    public delegate void XLuaMgr_Init(object obj);

    private Action outGame;
    public Action OutGame
    {
        get
        {
            return outGame;
        }
    }
    #endregion

    #region 数据
    private static XluaMgr instance;
    public static XluaMgr Instance
    {
        get
        {
            return instance;
        }
    }
    public LuaEnv luaEnv;
    private CSGameManager gameManager;
    private float lastLuaGcTime = 0;
    private float LuaGcInterval = 3;

    private LuaTable luaUIManager;
    public LuaTable LuaUIManager
    {
        get
        {
            if (luaUIManager == null)
            {
                if (luaEnv != null)
                {
                    luaUIManager = luaEnv.Global.Get<LuaTable>("uimanager");
                }
            }
            return luaUIManager;
        }
    }
    #endregion

    #region 构造函数
    public XluaMgr(CSGameManager gameManager)
    {
        this.gameManager = gameManager;
        instance = this;
        Init();
    }
    #endregion

    #region 初始化

    private void Init()
    {
        luaEnv = new LuaEnv();
        luaEnv.DoString("require 'main_in'");
        XLuaMgr_Init init = luaEnv.Global.Get<XLuaMgr_Init>("Init");
        if(init != null)
        {
            init(gameManager);
        }
        outGame = luaEnv.Global.Get<Action>("OutGame");
    }

    public override void Update()
    {
        float curTime = Time.time;
        if (curTime - lastLuaGcTime > LuaGcInterval) {
            Tick();
        }
    }

    private void Tick()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();
        }
    }

    public override void Destroy()
    {
        if (luaEnv != null)
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
        if (luaUIManager != null)
        {
            luaUIManager.Dispose();
            luaUIManager = null;
        }
    }
    #endregion
}
