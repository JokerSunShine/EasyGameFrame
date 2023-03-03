local Debug = CS.UnityEngine.Debug
local logEnable = CS.CSGameManager.instance.OpenLog

local function timestr()
    local time = Time.realtimeSinceStartup
    local seconds = math.floor(time)
    local minutes = seconds // 60
    seconds = seconds % 60
    local milliseconds = math.floor((time - seconds) * 1000)
    return string.format("[%02d:%02d.%03d]", minutes, seconds, milliseconds)
end

local function concat(...)
    local args = spack(...)
    for i = 1, args.n do
        args[i] = tostring(args[i])
    end
    local str =table.concat(args, "\t")
    str = '【LUALOG】' .. str
    return str
end

function log(...)
    if logEnable then
        Debug.Log(concat(...) .. "\n" .. debug.traceback())
    end
end

function logWarning(...)
    if logEnable then
        Debug.LogWarning(concat(...) .. "\n" .. debug.traceback())
    end
end

function logError(...)
    if logEnable then
        Debug.LogError(concat(...) .. "\n" .. debug.traceback())
    end
end
