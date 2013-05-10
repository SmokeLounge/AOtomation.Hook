// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadProcessMemory.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IReadProcessMemory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IReadProcessMemoryContract))]
    public interface IReadProcessMemory
    {
        #region Public Methods and Operators

        byte[] Read(IntPtr processHandle, IntPtr baseAddress, int size);

        byte ReadByte(IntPtr processHandle, IntPtr baseAddress);

        double ReadDouble(IntPtr processHandle, IntPtr baseAddress);

        short ReadInt16(IntPtr processHandle, IntPtr baseAddress);

        int ReadInt32(IntPtr processHandle, IntPtr baseAddress);

        long ReadInt64(IntPtr processHandle, IntPtr baseAddress);

        float ReadSingle(IntPtr processHandle, IntPtr baseAddress);

        string ReadString(IntPtr processHandle, IntPtr baseAddress, int? length = null);

        ushort ReadUInt16(IntPtr processHandle, IntPtr baseAddress);

        uint ReadUInt32(IntPtr processHandle, IntPtr baseAddress);

        ulong ReadUInt64(IntPtr processHandle, IntPtr baseAddress);

        #endregion
    }

    [ContractClassFor(typeof(IReadProcessMemory))]
    public abstract class IReadProcessMemoryContract : IReadProcessMemory
    {
        #region Public Methods and Operators

        public byte[] Read(IntPtr processHandle, IntPtr baseAddress, int size)
        {
            Contract.Requires<ArgumentException>(size >= 0);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == size);
            throw new NotImplementedException();
        }

        public byte ReadByte(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        public double ReadDouble(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        public short ReadInt16(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        public int ReadInt32(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        public long ReadInt64(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        public float ReadSingle(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        public string ReadString(IntPtr processHandle, IntPtr baseAddress, int? length = null)
        {
            Contract.Requires<ArgumentOutOfRangeException>(length == null || length >= 0);
            Contract.Ensures(Contract.Result<string>() != null);
            throw new NotImplementedException();
        }

        public ushort ReadUInt16(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        public uint ReadUInt32(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        public ulong ReadUInt64(IntPtr processHandle, IntPtr baseAddress)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}