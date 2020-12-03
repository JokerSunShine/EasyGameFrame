---@class Utility 通用
local Utility = {}

---@type table<table> 通用脚本列表
Utility.UtilityTables = {}

---加载通用脚本
function Utility.RegisterUtilities()
    Utility.Utility_Table = require 'luaRes.Common.Utility.Utility_Table'
end

---判断是否位空
---@param obj UnityEngine.Object
---@return boolean
function Utility.IsNull(obj)
    return CS.CommonUtility.IsNull(obj)
end

---判断字符串是否为空
---@param string string
---@return boolean
function Utility.IsNullOrEmpty(string)
    return CS.CommonUtility.IsNullOrEmpty(string)
end

return Utility