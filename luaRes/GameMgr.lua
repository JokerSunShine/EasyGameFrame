---@class GameMgr
local GameMgr = Class("GameMgr")

--region 数据
GameMgr.test = "测试1"
GameMgr.__get.test = function(self)
    log("获取数据" .. self.test)
end
GameMgr.__set.test = function(self,value)
    log("设置数据" .. value)
end
--endregion

--region 构造
function GameMgr:Init(data)
    log("游戏管理类进入" .. tostring(data))
end
--endregion

return GameMgr