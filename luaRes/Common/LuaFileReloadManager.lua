---@class LuaFileReloadManager 重载lua文件类
local LuaFileReloadManager = {}

---@type table<string> 屏蔽重载的lua路径
LuaFileReloadManager.AvoidLuaFilePathTable = {
    'luaRes.main'
}

---重载lua文件列表
---@param luaFileArray System.Array[string]
function LuaFileReloadManager.ReloadLuaFileArray(luaFileArray)
    local count = luaFileArray.Length - 1
    for k = 0,count do
        local filePath = luaFileArray[k]
        LuaFileReloadManager.ReloadSingleLuaFile(filePath)
    end
    ---重新加载其他数据

end

---重载单个Lua文件
---@param fileLuaPath string lua文件路径
function LuaFileReloadManager.ReloadSingleLuaFile(fileLuaPath)
    if Utility.IsNullOrEmpty(fileLuaPath) or Utility.IsContainValue(LuaFileReloadManager.AvoidLuaFilePathTable, fileLuaPath) then
        return
    end
    local oldTable = package.loaded[fileLuaPath]
    if oldTable == nil then
        return
    end
    local reloadTableResult = pcall(LuaFileReloadManager.ReloadLuaFile, fileLuaPath)
    if reloadTableResult == false then
        package.loaded[fileLuaPath] = oldTable
    else
        LuaFileReloadManager.CoverOtherSameFile(fileLuaPath,oldTable,package.loaded[fileLuaPath])
    end
end

---重载Lua文件
---@param fileLuaPath string
function LuaFileReloadManager.ReloadLuaFile(fileLuaPath)
    package.loaded[fileLuaPath] = nil
    require(fileLuaPath)
end

---覆盖其他所有相同的Lua脚本
---@param fileLuaPath string
---@param oldTable table
---@param newTable table
function LuaFileReloadManager.CoverOtherSameFile(fileLuaPath,oldTable,newTable)
    if Utility.IsNullOrEmpty(fileLuaPath) or type(oldTable) ~= 'table' then
        return
    end
    for k,v in pairs(package.loaded) do
        if k ~= fileLuaPath and v and getmetatable(v) == oldTable then
            setmetatable(v,newTable)
        end
    end
end

return LuaFileReloadManager