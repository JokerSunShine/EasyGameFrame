﻿#if !NO_RUNTIME
using System;
using CustomDataStruct;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
#endif


namespace ProtoBuf.Serializers
{
    internal sealed class UInt32Serializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof(uint);
#endif
        public UInt32Serializer(ProtoBuf.Meta.TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(uint));
#endif
        }
        public Type ExpectedType { get { return expectedType; } }

        bool IProtoSerializer.RequiresOldValue { get { return false; } }
        bool IProtoSerializer.ReturnsValue { get { return true; } }
#if !FEAT_IKVM
        public object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return ValueObject.Get(source.ReadUInt32());
        }
        public void Write(object value, ProtoWriter dest)
        {
            ProtoWriter.WriteUInt32(ValueObject.Value<uint>(value), dest);
        }
#endif
#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(Compiler.CompilerContext ctx, Compiler.Local valueFrom)
        {
            ctx.EmitBasicWrite("WriteUInt32", valueFrom);
        }
        void IProtoSerializer.EmitRead(Compiler.CompilerContext ctx, Compiler.Local valueFrom)
        {
            ctx.EmitBasicRead("ReadUInt32", ctx.MapType(typeof(uint)));
        }
#endif
    }
}
#endif