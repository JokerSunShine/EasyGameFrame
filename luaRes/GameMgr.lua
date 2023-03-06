---@class GameMgr:LuaObject
local GameMgr = Class("GameMgr")

--region 数据
GameMgr.test1 = "测试1"
GameMgr.__get.test = function(self)
    log("获取数据" .. self.test1)
    return self.test1
end
GameMgr.__set.test = function(self,value)
    self.test1 = value
    log("设置数据" .. value)
end
--endregion

--region 构造
function GameMgr:Init(data)
    log("游戏管理类进入" .. tostring(data))
end
--endregion

return GameMgr