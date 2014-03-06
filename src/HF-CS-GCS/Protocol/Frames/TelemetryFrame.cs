// GENERATED CODE

using System;
using System.Collections.Generic;

namespace HF_CS_GCS.Protocol.Frames
{
    class TelemetryStruct : FrameStruct
    {
        public double? Temperature = null;
        public int? DummyIntData = null;
        public double? WindSpeed = null;
     
        public override int TotalSize
        {
            get { return sizeof (double) + sizeof (int) + sizeof (double); }
        }
    }

    class TelemetryFrame : Frame
    {
        protected override int FieldCount
        {
            get { return 3; }
        }

        public override FrameStruct Parse(List<byte> bytes)
        {
            byte[] data = bytes.ToArray();
            bool[] fields = PreParseData(data);
            int iterator = 0;

            var fStruct = new TelemetryStruct();

            if (fields[0])
            {
                fStruct.Temperature = BitConverter.ToDouble(data, iterator + 5);
                iterator += Utils.GetSize(fStruct.Temperature.GetValueOrDefault());
            }
            if (fields[1])
            {
                fStruct.DummyIntData = BitConverter.ToInt32(data, iterator + 5);
                iterator += Utils.GetSize(fStruct.DummyIntData.GetValueOrDefault());
            }
            if (fields[2])
            {
                fStruct.WindSpeed = BitConverter.ToDouble(data, iterator + 5);
                iterator += Utils.GetSize(fStruct.WindSpeed.GetValueOrDefault()); ;
            }

            CheckCrcSum(data[iterator]);

            return fStruct;
        }
    }
}
