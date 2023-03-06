local luaObject = require 'luaRes.LuaCore.LuaClass.Base.LuaObject'

---@type table<string,LuaObject>
ClassTemplateList = {}

---@param className string
---@param base table
---@return LuaObject
function Class(className, base)
    if className == nil then
        return
    end
    local cls = {}
    base = base or luaObject
    setmetatable(cls, {__index = base})
    cls.__className = className or "default"
    cls.__base = base
    ClassTemplateList[className] = cls
    return cls
end

---@param tbl table
function GCCallBack(tbl)
    if tbl == nil then
        return
    end
    if tbl.OnDestruct ~= nil then
        tbl:OnDestruct()
    end
end