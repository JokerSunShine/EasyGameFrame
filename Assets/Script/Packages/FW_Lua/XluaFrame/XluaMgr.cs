﻿using System;
using UnityEngine;
using XLua;
public class XluaMgr:ManagerObject
{
    #region 委托类型
    [CSharpCallLua]
    public delegate void XLuaMgr_Init(object obj);
    [CSharpCallLua]
    public delegate void XLuaMgr_Update();

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
    private Main gameManager;
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

    private XLuaMgr_Update luaUpdate;
    private XLuaMgr_Update LuaUpdate
    {
        get
        {
            if(luaUpdate == null)
            {
                luaUpdate = luaEnv.Global.Get<XLuaMgr_Update>("Update");
            }

            return luaUpdate;
        }
    }
    #endregion

    #region 构造函数
    public XluaMgr(Main gameManager)
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
        luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
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

        LuaUpdate();
    }

    private void Tick()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();
        }
    }
    #endregion
}
