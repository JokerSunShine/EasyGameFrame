---@type Utility
local Utility = Utility

---是否包含指定的value
---@param table table
---@param value any
---@return boolean
function Utility.IsContainValue(table,value)
    if type(table) ~= 'table' then
        return false
    end
    for k,v in pairs(table) do
        if v == value then
            return true
        end
    end
    return false
end

---获取表内数据数量
---@param table table
---@return number
function Utility.GetTableCount(table)
    if type(table) ~= 'table' then
        return 0
    end
    local num = 0
    for k,v in pairs(table) do
        num = num + 1
    end
    return num
end