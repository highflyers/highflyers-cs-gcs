using System;
using System.Collections.Generic;
using System.IO;
using HighFlyers.CsGCS.Protocol.Frames;

namespace HighFlyers.CsGCS.Protocol
{
    enum FrameType
    {
        Telemetry = 0
    }

    abstract class FrameStruct
    {
        public abstract int TotalSize { get; }
    }

    abstract class Frame
    {
        protected abstract int FieldCount { get; }

        protected void CheckCrcSum(byte crc)
        {
            if (crc != 12)
                throw new InvalidDataException("Invalid crc sum!");
        }

        protected bool[] PreParseData(byte[] data)
        {
            UInt16 size = BitConverter.ToUInt16(data, 1);

            if (size != data.Length)
                throw new InvalidDataException("Invalid data length");

            UInt16 fieldFlags = BitConverter.ToUInt16(data, 3);
            var fields = new bool[FieldCount];

            for (int i = 0; i < FieldCount; i++)
                fields[i] = (fieldFlags & (1 >> i)) != 0;

            return fields;
        }


        public abstract FrameStruct Parse(List<byte> bytes);
    }

    class FrameBuilder
    {
        public static Frame BuildFrame(List<byte> data)
        {
            Frame frame;

            switch ((FrameType)data[0])
            {
                case FrameType.Telemetry:
                    frame = new TelemetryFrame();
                    break;
                default:
                    throw new InvalidDataException("Unexpected frame type");
            }

            frame.Parse(data);
            return frame;
        }
    }
}
