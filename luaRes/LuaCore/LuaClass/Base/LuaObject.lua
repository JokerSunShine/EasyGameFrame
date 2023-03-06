local LuaObjectEx = require 'luaRes.LuaCore.LuaClass.Base.LuaObjectEx'

---@class LuaObject
local LuaObject = {}

--region 数据
---@public
---@type string
LuaObject.__className = nil

---@private
---@type table|function
LuaObject.__index = nil

---@private
---@type table
LuaObject.__base = nil

---@public
---@type UnityEngine.GameObject
LuaObject.go = nil

---@protected
---@type function
LuaObject.__get = {}

---@protected
---@type function
LuaObject.__set = {}

LuaObject.__gc = GCCallBack
--endregion

--region 功能
function LuaObject:New(...)
    return self:NewWithGO(nil,...)
end

function LuaObject:NewWithGO(go,...)
    local instanceCls = {}
    instanceCls.go = go
    for k,v in pairs(self) do
        instanceCls[k] = v
    end
    setmetatable(instanceCls,LuaObjectEx)
    if instanceCls.Init then
        instanceCls:Init(...)
    end
    return instanceCls
end
--endregion

function LuaObject:Init()

end

return LuaObject

