using System;
using System.IO;
public class ProtoBufManager:I_ProtoBufManager
{
    public static void Serializer<T>(Stream destination, T instance)
    {
        ProtoBuf.Serializer.Serialize(destination,instance);
    }
    
    public static object Deserializer(byte[] buffs,Type type,int msgId)
    {
        object obj = null;
        if(type != null)
        {
            MemoryStream stream = new MemoryStream(buffs);
            obj = ProtoBuf.Serializer.Deserialize<Type>(stream);
        }
        return obj;
    }
}