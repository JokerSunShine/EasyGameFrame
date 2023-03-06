---@class LuaObjectEx luaObject扩展
local LuaObjectEx = {}

LuaObjectEx.__index = function(t, k)
    if ClassTemplateList[t.__className][k] then
        return ClassTemplateList[t.__className][k]
    end
    if t.__get[k] then
        return t.__get[k](t)
    end
end

LuaObjectEx.__newindex = function(t, k, v)
    if t.__set[k] then
        t.__set[k](t, v)
        return
    end
    rawset(t, k, v)
end

return LuaObjectEx