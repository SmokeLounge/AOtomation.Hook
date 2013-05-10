// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadProcessMemory.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the ReadProcessMemory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics.Contracts;
    using System.Text;

    using SmokeLounge.AOtomation.Hook.Kernel32;

    [Export(typeof(IReadProcessMemory))]
    public class ReadProcessMemory : IReadProcessMemory
    {
        #region Public Methods and Operators

        public byte[] Read(IntPtr processHandle, IntPtr baseAddress, int size)
        {
            var buffer = new byte[size];
            using (var pinnedBuffer = new PinnedObject(buffer))
            {
                uint bytesRead;
                NativeMethods.ReadProcessMemory(
                    processHandle, baseAddress, pinnedBuffer.AddrOfPinnedObject, (uint)size, out bytesRead);
                return buffer;
            }
        }

        public byte ReadByte(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, 1);
            return array[0];
        }

        public double ReadDouble(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, sizeof(double));
            return BitConverter.ToDouble(array, 0);
        }

        public short ReadInt16(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, sizeof(short));
            return BitConverter.ToInt16(array, 0);
        }

        public int ReadInt32(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, sizeof(int));
            return BitConverter.ToInt32(array, 0);
        }

        public long ReadInt64(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, sizeof(long));
            return BitConverter.ToInt64(array, 0);
        }

        public float ReadSingle(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, sizeof(float));
            return BitConverter.ToSingle(array, 0);
        }

        public string ReadString(IntPtr processHandle, IntPtr baseAddress, int? length = null)
        {
            var maxSize = length ?? 100;
            Contract.Assume(maxSize >= 0);
            var array = this.Read(processHandle, baseAddress, maxSize);

            var str = Encoding.ASCII.GetString(array);
            var nullChar = str.IndexOf((char)0);
            return nullChar >= 0 ? str.Substring(0, nullChar) : str;
        }

        public ushort ReadUInt16(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, sizeof(ushort));
            return BitConverter.ToUInt16(array, 0);
        }

        public uint ReadUInt32(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, sizeof(uint));
            return BitConverter.ToUInt32(array, 0);
        }

        public ulong ReadUInt64(IntPtr processHandle, IntPtr baseAddress)
        {
            var array = this.Read(processHandle, baseAddress, sizeof(ulong));
            return BitConverter.ToUInt64(array, 0);
        }

        #endregion
    }
}