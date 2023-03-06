package.path = package.path .. ";" .. CS.UnityEngine.Application.persistentDataPath .. "/?.lua"
-----c#补丁
util = require 'xlua.util'
require 'luaRes.Common.LuaFileReloadManager'
Utility = require 'luaRes.Common.Utility.Utility'
--**********UnityEngine****************
require 'luaRes.UnityEngine.UnityEngineLoad'
--**********LuaCore****************
require 'luaRes.LuaCore.LuaCoreLoad'
--**********GameMgr****************
GameMgr = require 'luaRes.GameMgr'
--**********Common****************
require 'luaRes.Common.CommonLoad'

---初始化lua
---在C#中调用,初始化lua
function Init(gameManager)
    print("初始化进入" .. tostring(gameManager))
    Utility.RegisterUtilities()
    local gameMgr = GameMgr:New("创建副本")
    local gameMgr2 = GameMgr:New("测试",gameMgr)
    gameMgr2.test2 = 10
    log(gameMgr2.test2)
end