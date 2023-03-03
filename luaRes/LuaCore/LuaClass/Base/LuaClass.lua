local mt = {}

function Class(clsName, base)
    local cls = {}
    base = base or mt
    cls.__get = {}
    cls.__set = {}
    setmetatable(cls, {__index = base})
    cls.clsName = clsName or "default"
    cls.base = base
    cls.new = function(...)
        local cls_instance = {}
        cls_instance.getset_values = {}
        for k,v in pairs(cls) do
            cls_instance[k] = v
        end
        local cls_instance_mt = {
            __index = function(t, k)
                if cls[k] then
                    return cls[k]
                end
                if t.__get[k] then
                    t.__get[k](t)
                    return t.getset_values[k]
                end
                if string.sub(k, 1, 2) == "__" then
                    local tmpK = string.sub(k, 3, -1)
                    if t.getset_values[tmpK] then
                        return t.getset_values[tmpK]
                    end
                end
            end,
            __newindex = function(t, k, v)
                if t.__set[k] then
                    t.__set[k](t, v)
                    cls_instance.getset_values[k] = v
                    return
                end
                if string.sub(k, 1, 2) == "__" then
                    local tmpK = string.sub(k, 3, -1)
                    if t.getset_values[tmpK] then
                        t.getset_values[tmpK] = v
                    end
                end
                rawset(t, k, v)
            end
        }
        setmetatable(cls_instance, cls_instance_mt)
        if cls_instance.Init then
            cls_instance:Init(...)
        end
        return cls_instance
    end
    return cls
end
