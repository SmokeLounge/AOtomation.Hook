// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IpcServerChannel.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IpcServerChannel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook.Communication.NamedPipe
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.IO.Pipes;
    using System.Security.AccessControl;
    using System.Text;
    using System.Threading;

    public sealed class IpcServerChannel : IIpcServerChannel
    {
        #region Fields

        private readonly string channelName;

        private readonly int inBufferSize;

        private readonly int outBufferSize;

        private bool isRunning;

        private NamedPipeServerStream pipeServer;

        #endregion

        #region Constructors and Destructors

        public IpcServerChannel(string channelName)
        {
            Contract.Requires<ArgumentException>(string.IsNullOrWhiteSpace(channelName) == false);

            this.channelName = channelName;

            this.inBufferSize = 16384;
            this.outBufferSize = 16384;
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get
            {
                return this.channelName;
            }
        }

        public Action<byte[], Action> PacketReceivedCallback { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            this.Stop();
        }

        public void Start()
        {
            this.pipeServer = null;

            try
            {
                var pipeSecurity = this.CreateSystemIOPipeSecurity();

                this.pipeServer = new NamedPipeServerStream(
                    this.Name, 
                    PipeDirection.InOut, 
                    NamedPipeServerStream.MaxAllowedServerInstances, 
                    PipeTransmissionMode.Message, 
                    PipeOptions.WriteThrough, 
                    this.inBufferSize, 
                    this.outBufferSize, 
                    pipeSecurity, 
                    HandleInheritability.None);

                this.isRunning = true;

                // TODO: Create thread manually if it fails to Stop()
                ThreadPool.QueueUserWorkItem(this.ListenerThread, null);
            }
            catch (Exception)
            {
                if (this.pipeServer == null)
                {
                    return;
                }

                this.pipeServer.Dispose();
                this.pipeServer = null;
            }
        }

        public void Stop()
        {
            if (this.pipeServer == null)
            {
                return;
            }

            if (this.pipeServer.IsConnected)
            {
                this.pipeServer.Disconnect();
            }

            this.pipeServer.Dispose();
            this.pipeServer = null;
        }

        #endregion

        #region Methods

        private PipeSecurity CreateSystemIOPipeSecurity()
        {
            var pipeSecurity = new PipeSecurity();

            // Allow Everyone read and write access to the pipe.
            pipeSecurity.SetAccessRule(
                new PipeAccessRule("Authenticated Users", PipeAccessRights.ReadWrite, AccessControlType.Allow));

            // Allow the Administrators group full access to the pipe.
            pipeSecurity.SetAccessRule(
                new PipeAccessRule("Administrators", PipeAccessRights.FullControl, AccessControlType.Allow));

            return pipeSecurity;
        }

        private void ListenerThread(object state)
        {
            Contract.Requires(this.pipeServer != null);

            var resumeResponse = Encoding.ASCII.GetBytes("cont");
            var resumeResponseLength = resumeResponse.Length;

            while (this.isRunning)
            {
                if (this.pipeServer.IsConnected == false)
                {
                    this.pipeServer.WaitForConnection();
                }

                var message = new byte[this.inBufferSize];
                var messageWriteIndex = 0;
                var freeBufferSpace = this.inBufferSize;
                do
                {
                    var bytesRead = this.pipeServer.Read(message, messageWriteIndex, freeBufferSpace);
                    messageWriteIndex += bytesRead;
                    freeBufferSpace -= bytesRead;
                }
                while (!this.pipeServer.IsMessageComplete);

                var responseSent = false;

                Action resumeHookAction = () =>
                    {
                        if (this.pipeServer.IsConnected == false)
                        {
                            this.pipeServer.Disconnect();
                            responseSent = true;
                            return;
                        }

                        this.pipeServer.Write(resumeResponse, 0, resumeResponseLength);
                        this.pipeServer.WaitForPipeDrain();
                        responseSent = true;
                    };

                if (this.PacketReceivedCallback != null)
                {
                    var trim = new byte[messageWriteIndex];
                    Array.Copy(message, trim, messageWriteIndex);
                    this.PacketReceivedCallback(trim, resumeHookAction);
                }

                if (responseSent == false)
                {
                    resumeHookAction();
                }
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(string.IsNullOrWhiteSpace(this.channelName) == false);
            Contract.Invariant(this.inBufferSize >= 0);
            Contract.Invariant(this.outBufferSize >= 0);
        }

        #endregion
    }
}