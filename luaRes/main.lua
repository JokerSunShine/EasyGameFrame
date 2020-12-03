package.path = package.path .. ";" .. CS.UnityEngine.Application.persistentDataPath .. "/?.lua"
-----c#补丁
util = require 'xlua.util'
---@type LuaFileReloadManager
LuaFileReloadManager = require 'luaRes.Common.LuaFileReloadManager'
---@type Utility
Utility = require 'luaRes.Common.Utility.Utility'

---初始化lua
---在C#中调用,初始化lua
function Init(gameManager)
    print("初始化进入" .. tostring(gameManager))
    Utility.RegisterUtilities()
end