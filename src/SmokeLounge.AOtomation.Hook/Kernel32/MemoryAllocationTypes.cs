// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryAllocationTypes.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the MemoryAllocationTypes type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook.Kernel32
{
    using System;

    [Flags]
    internal enum MemoryAllocationTypes : uint
    {
        Commit = 0x00001000, 

        Reserve = 0x00002000, 

        Reset = 0x00080000, 

        ResetUndo = 0x1000000, 

        LargePages = 0x20000000, 

        Physical = 0x00400000, 

        TopDown = 0x00100000
    }
}