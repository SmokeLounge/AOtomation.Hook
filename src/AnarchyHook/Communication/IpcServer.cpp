#include "stdafx.h"
#include "IpcServer.h"

#include <functional>
#include <process.h>
#include <string>

#include "Threading/Thread.h"

namespace AnarchyHook {
namespace Communication {

using namespace AnarchyHook::Threading;

IpcServer::IpcServer(string pipeName, ReceiveCallback receivedCallback) :
    inBufferSize(2048),
    outBufferSize(2048),
    pipeName(pipeName),
    pipeHandle(nullptr),
    receivedCallback(receivedCallback),
    workerThread(nullptr) {}

IpcServer::~IpcServer(void) {
    this->Stop();
}

bool IpcServer::IsRunning() const {
    return this->isRunning;
}

string IpcServer::PipeName() const {
    return this->pipeName;
}

void IpcServer::Start() {
    this->pipeHandle = this->CreatePipe();
    if (this->pipeHandle == nullptr) {
        return;
    }

    this->isRunning = true;
    auto threadStart = bind(&IpcServer::Run, this);
    this->workerThread = ThreadPtr(new Thread(threadStart));
    this->workerThread->Create();
    this->workerThread->Start();
}

void IpcServer::Stop() {
    if (this->isRunning == false) {
        return;
    }

    this->isRunning = false;
    this->workerThread->Shutdown();
    this->workerThread = nullptr;
}

HANDLE IpcServer::CreatePipe() {
    SECURITY_ATTRIBUTES sa;
    sa.lpSecurityDescriptor = reinterpret_cast<PSECURITY_DESCRIPTOR>(new char[SECURITY_DESCRIPTOR_MIN_LENGTH]);
    if (!InitializeSecurityDescriptor(sa.lpSecurityDescriptor, SECURITY_DESCRIPTOR_REVISION)) {
        DWORD er = ::GetLastError();
    }

    if (!SetSecurityDescriptorDacl(sa.lpSecurityDescriptor, TRUE, (PACL)0, FALSE)) {
        DWORD er = ::GetLastError();
    }

    sa.nLength = sizeof sa;
    sa.bInheritHandle = TRUE;

    auto name = string("\\\\.\\pipe\\") + this->pipeName;
    HANDLE hPipe = ::CreateNamedPipe(
                       name.c_str(),
                       PIPE_ACCESS_INBOUND | FILE_FLAG_WRITE_THROUGH,
                       PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT,
                       PIPE_UNLIMITED_INSTANCES,
                       this->outBufferSize,
                       this->inBufferSize,
                       NMPWAIT_USE_DEFAULT_WAIT,
                       &sa);
    if (hPipe == INVALID_HANDLE_VALUE) {
        return nullptr;
    }

    return hPipe;
}

void IpcServer::OnReceived(const char* message, const int& size) {
    if (this->receivedCallback == nullptr) {
        return;
    }

    this->receivedCallback(message, size);
}

void IpcServer::Run() {
    while (this->isRunning) {
        BOOL  bResult = ::ConnectNamedPipe(this->pipeHandle, 0);
        DWORD dwError = GetLastError();

        if (bResult || dwError == ERROR_PIPE_CONNECTED) {
            char buffer[1024] = {0 };
            DWORD read = 0;

            if (!(::ReadFile(this->pipeHandle, &buffer, this->inBufferSize, &read, 0))) {
                unsigned int error = GetLastError();
            }

            if (read > 0) {
                this->OnReceived(buffer, read);
            }

			::DisconnectNamedPipe(this->pipeHandle);
        } else {}

        ::Sleep(0);
    }
}

}
}
