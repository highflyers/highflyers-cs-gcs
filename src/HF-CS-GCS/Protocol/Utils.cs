using System;
using System.Data;

namespace HighFlyers.CsGCS.Protocol
{
    class Utils
    {
        public static int GetSize<T>(T value)
        {
            var v = value as FrameStruct;
            
            if (v != null)
                return v.TotalSize;

            if (typeof(T) == typeof(byte))
                return sizeof(byte);
            if (typeof(T) == typeof(UInt16))
                return sizeof(UInt16);
            if (typeof(T) == typeof(UInt32))
                return sizeof(UInt32);
            if (typeof(T) == typeof(UInt64))
                return sizeof(UInt64);
            if (typeof(T) == typeof(Int16))
                return sizeof(Int16);
            if (typeof(T) == typeof(Int32))
                return sizeof(Int32);
            if (typeof(T) == typeof(Int64))
                return sizeof(Int64);
            
            throw new ConstraintException("Cannot get object size");
        }
    }
}
