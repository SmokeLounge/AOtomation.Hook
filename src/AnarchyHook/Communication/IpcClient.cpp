#include "stdafx.h"
#include "IpcClient.h"

#include <memory>

namespace AnarchyHook {
namespace Communication {

IpcClient::IpcClient(string pipeName) :
    inBufferSize(4),
    pipeHandle(nullptr),
    pipeName(pipeName) {
    this->inBuffer = BufferPtr(new BYTE[this->inBufferSize]);
}

IpcClient::~IpcClient(void) {
    this->Disconnect();
}

bool IpcClient::IsConnected() const {
    return this->pipeHandle != nullptr;
}

void IpcClient::Connect() {
    this->pipeHandle = this->CreatePipe();
}

void IpcClient::Disconnect() {
    if (this->IsConnected() == false) {
        return;
    }

    ::CloseHandle(this->pipeHandle);
    this->pipeHandle = nullptr;
}

void IpcClient::Send(const void* pMessage, const int& pSize) {
    if (this->IsConnected() == false) {
        return;
    }

    DWORD written(0);
    BOOL result = ::WriteFile(this->pipeHandle, pMessage, pSize, &written, NULL);
    if (result == FALSE) {
        return;
    }

    BOOL finishRead(FALSE);
    DWORD read(0);
    do {
        finishRead = ::ReadFile(this->pipeHandle, this->inBuffer.get(), this->inBufferSize, &read, NULL);
        if (finishRead == FALSE && ::GetLastError() != ERROR_MORE_DATA) {
            return;
        }
    } while(finishRead == FALSE);
}

HANDLE IpcClient::CreatePipe() {
    HANDLE hPipe(nullptr);
    while (true) {
        auto name = string("\\\\.\\pipe\\") + this->pipeName;
        hPipe = ::CreateFile(
                    name.c_str(),
                    GENERIC_READ | GENERIC_WRITE,
                    0,
                    NULL,
                    OPEN_EXISTING,
                    0,
                    NULL);

        if (hPipe != INVALID_HANDLE_VALUE) {
            break;
        }

        if (::GetLastError() != ERROR_PIPE_BUSY) {
            return nullptr;
        }

        if (::WaitNamedPipe(this->pipeName.c_str(), 5000) == FALSE) {
            return nullptr;
        }
    }

    auto mode = (DWORD)PIPE_READMODE_MESSAGE;
    if (::SetNamedPipeHandleState(hPipe, &mode, NULL, NULL) == FALSE) {
        return nullptr;
    }

    return hPipe;
}

}
}
