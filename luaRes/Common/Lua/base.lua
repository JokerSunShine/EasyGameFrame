if not pack then
    pack = table.pack
end

if not unpack then
    unpack = table.unpack
end

-- 解决原生pack的nil截断问题，spack与sunpack要成对使用
function spack(...)
    local params = {...}
    params.n = select('#', ...)
    return params
end

function sunpack(params)
    return unpack(params, 1, params.n)
end

function trequire(path)
    local e, r = pcall(require, path)
    return e and r
end

function rrequire(path)
    package.loaded[path] = nil
    return require(path)
end

function scall(func, ...)
    if not func then
        return
    end

    local args = spack(...)
    if #args == 0 then
        return xpcall(func, logError)
    else
        return xpcall(function ()
            return func(sunpack(args))
        end, logError)
    end
end

local enumId = 0
function enum(n)
    enumId = n or enumId + 1
    return enumId
end

function bind(self, callback)
    return callback and function (...)
        return callback(self, ...)
    end
end

local itpool = {}
local function allocit(t)
    local it = table.remove(itpool)
    if it then
        it[1] = t
        it[2] = t
    else
        it = {t, t}
    end
    return it
end

local function recycleit(it)
    it[1] = nil
    it[2] = nil
    table.insert(itpool, it)
end

local function contains(t1, t2, key)
    while t1 ~= t2 do
        if rawget(t1, key) then
            return true
        end
        t1 = getmetatable(t1).__index
    end
    return false
end

local function mpairsIter(it, key)
    local t = it[2]
    local k, v = next(t, key)
    while k do
        if not contains(it[1], t, k) then
            return k, v
        end
        k, v = next(t, k)
    end

    local mt = getmetatable(t)
    if mt then
        t = mt.__index
        if t then
            it[2] = t
            return mpairsIter(it)
        end
    end

    recycleit(it)
    return nil
end

function mpairs(t)
    local it = allocit(t)
    return mpairsIter, it, nil
end