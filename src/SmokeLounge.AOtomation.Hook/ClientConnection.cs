// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientConnection.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the ClientConnection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO.Pipes;

    using SmokeLounge.AOtomation.Hook.Communication.NamedPipe;

    public sealed class ClientConnection : IClientConnection
    {
        #region Fields

        private readonly string hookServerChannelName;

        private readonly IReadProcessMemory readProcessMemory;

        private readonly IIpcServerChannel receiveCallbackChannel;

        private readonly IntPtr remoteProcessHandle;

        private readonly IIpcServerChannel sendCallbackChannel;

        private Action<byte[], Action> receiveCallback;

        private Action<byte[], Action> sendCallback;

        #endregion

        #region Constructors and Destructors

        public ClientConnection(
            IntPtr remoteProcessHandle, 
            string hookServerChannelName, 
            IIpcServerChannel sendCallbackChannel, 
            IIpcServerChannel receiveCallbackChannel, 
            IReadProcessMemory readProcessMemory)
        {
            Contract.Requires<ArgumentException>(string.IsNullOrWhiteSpace(hookServerChannelName) == false);
            Contract.Requires<ArgumentNullException>(sendCallbackChannel != null);
            Contract.Requires<ArgumentNullException>(receiveCallbackChannel != null);
            Contract.Requires<ArgumentNullException>(readProcessMemory != null);

            this.remoteProcessHandle = remoteProcessHandle;
            this.hookServerChannelName = hookServerChannelName;
            this.sendCallbackChannel = sendCallbackChannel;
            this.receiveCallbackChannel = receiveCallbackChannel;
            this.readProcessMemory = readProcessMemory;
        }

        #endregion

        #region Public Properties

        public Action<byte[], Action> ReceiveCallback
        {
            get
            {
                return this.receiveCallback;
            }

            set
            {
                this.receiveCallback = value;
                if (this.receiveCallback == null)
                {
                    this.receiveCallbackChannel.PacketReceivedCallback = null;
                    return;
                }

                this.receiveCallbackChannel.PacketReceivedCallback = this.OnReceivedCallback;
            }
        }

        public Action<byte[], Action> SendCallback
        {
            get
            {
                return this.sendCallback;
            }

            set
            {
                this.sendCallback = value;
                if (this.sendCallback == null)
                {
                    this.sendCallbackChannel.PacketReceivedCallback = null;
                    return;
                }

                this.sendCallbackChannel.PacketReceivedCallback = this.OnSendCallback;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            this.sendCallbackChannel.Dispose();
            this.receiveCallbackChannel.Dispose();
        }

        public void Send(byte[] message)
        {
            using (
                var pipeClientStream = new NamedPipeClientStream(
                    ".", this.hookServerChannelName, PipeDirection.Out, PipeOptions.WriteThrough))
            {
                pipeClientStream.Connect();
                pipeClientStream.Write(message, 0, message.Length);
                pipeClientStream.WaitForPipeDrain();
            }
        }

        public void Start()
        {
            this.sendCallbackChannel.PacketReceivedCallback = this.OnSendCallback;
            this.receiveCallbackChannel.PacketReceivedCallback = this.OnReceivedCallback;
            this.sendCallbackChannel.Start();
            this.receiveCallbackChannel.Start();
        }

        #endregion

        #region Methods

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.sendCallbackChannel != null);
            Contract.Invariant(this.receiveCallbackChannel != null);
            Contract.Invariant(this.readProcessMemory != null);
        }

        private void OnReceivedCallback(byte[] buffer, int size, Action resumeHook)
        {
            Contract.Assume(resumeHook != null);
            if (this.ReceiveCallback == null)
            {
                resumeHook();
                return;
            }

            if (buffer == null || buffer.Length < 8)
            {
                resumeHook();
                return;
            }

            Contract.Assume(buffer.Length == 8);
            var address = BitConverter.ToUInt32(buffer, 0);
            var readSize = BitConverter.ToUInt32(buffer, 4);
            Contract.Assume(readSize > 0);
            var packet = this.readProcessMemory.Read(this.remoteProcessHandle, new IntPtr(address), (int)readSize);
            Contract.Assume(this.ReceiveCallback != null);
            this.ReceiveCallback(packet, resumeHook);
        }

        private void OnSendCallback(byte[] buffer, int size, Action resumeHook)
        {
            Contract.Assume(resumeHook != null);
            if (this.SendCallback == null)
            {
                resumeHook();
                return;
            }

            if (buffer == null || buffer.Length < 8)
            {
                resumeHook();
                return;
            }

            Contract.Assume(buffer.Length == 8);
            var address = BitConverter.ToUInt32(buffer, 0);
            var readSize = BitConverter.ToUInt32(buffer, 4);
            var packet = this.readProcessMemory.Read(this.remoteProcessHandle, new IntPtr(address), (int)readSize);
            Contract.Assume(this.SendCallback != null);
            this.SendCallback(packet, resumeHook);
        }

        #endregion
    }
}