using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace HF_CS_GCS.Protocol
{
    class FrameParsedArgs : EventArgs
    {
        public Frame ParsedFrame { get; set; }
    }

    class Parser
    {
        private const byte EndFrame = 12;
        private const byte Sentinel = 13;
        private const int MaxLength = 2048;
        private bool prevSentinel = false;

        readonly List<byte> bytes = new List<byte>(); 
        
        internal delegate void FrameParsedHandler(object sender, FrameParsedArgs args);
        public event FrameParsedHandler FrameParsed;

        public void AppendBytes(byte[] data)
        {
            foreach (byte b in data)
                AppendByte(b);
        }

        private void AppendByte(byte b)
        {
            if (prevSentinel)
            {
                if (b == Sentinel || b == EndFrame)
                    bytes.Add(b);
                else 
                    throw new InvalidDataException("Unexpected token " + b);
            }
            else if (b == Sentinel)
                prevSentinel = true;
            else
                bytes.Add(b);

            if (b == EndFrame)
                ParseFrame();
            else if (MaxLength == bytes.Count)
            {
                bytes.Clear();
                throw new WarningException("Too many bytes without end_frame sign. Dropping data...");
            }
        }

        private void ParseFrame()
        {
            
        }
    }
}
