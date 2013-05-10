// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PinnedObject.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the PinnedObject type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Runtime.InteropServices;

    public sealed class PinnedObject : IDisposable
    {
        #region Fields

        private readonly GCHandle? gcHandle;

        private readonly object target;

        #endregion

        #region Constructors and Destructors

        public PinnedObject(object value)
        {
            this.target = value;
            this.gcHandle = GCHandle.Alloc(this.target, GCHandleType.Pinned);
        }

        #endregion

        #region Public Properties

        public IntPtr AddrOfPinnedObject
        {
            get
            {
                if (this.gcHandle.HasValue == false)
                {
                    return IntPtr.Zero;
                }

                return this.gcHandle.Value.AddrOfPinnedObject();
            }
        }

        public object Target
        {
            get
            {
                return this.target;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            if (this.gcHandle.HasValue && this.gcHandle.Value.IsAllocated)
            {
                this.gcHandle.Value.Free();
            }
        }

        #endregion
    }
}