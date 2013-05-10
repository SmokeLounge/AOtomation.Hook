#include "StdAfx.h"
#include "Win32Thread.h"

#include <process.h>

#include "Lock.h"

namespace AnarchyHook {
namespace Threading {

Win32Thread::Win32Thread() :
    threadHandle(0),
    id(0),
    canRun(true),
    suspended(true) {}

Win32Thread::~Win32Thread() {
    if (this->threadHandle) {
        CloseHandle(this->threadHandle);
    }
}

bool Win32Thread::CanRun() {
    Lock guard(this->mutex);
    return this->canRun;
}

unsigned int Win32Thread::Id() const {
    return this->id;
}

bool Win32Thread::Create(unsigned int stackSize) {
    this->threadHandle = reinterpret_cast<HANDLE>(_beginthreadex(0, stackSize, threadFunc, this, CREATE_SUSPENDED, &this->id));

    if (this->threadHandle) {
        return true;
    }

    return false;
}

void Win32Thread::Join() {
    WaitForSingleObject(this->threadHandle, INFINITE);
}

void Win32Thread::Resume() {
    if (this->suspended) {
        Lock guard(this->mutex);

        if (suspended) {
            ResumeThread(threadHandle);
            suspended = false;
        }
    }
}

void Win32Thread::Shutdown() {
    if (this->canRun) {
        Lock guard(this->mutex);

        if (this->canRun) {
            this->canRun = false;
        }

        this->Resume();
    }
}

void Win32Thread::Start() {
    this->Resume();
}

void Win32Thread::Suspend() {
    if (!suspended) {
        Lock guard(this->mutex);

        if (!this->suspended) {
            SuspendThread(this->threadHandle);
            this->suspended = true;
        }
    }
}

unsigned int __stdcall Win32Thread::threadFunc(void *args) {
    auto pThread = reinterpret_cast<Win32Thread*>(args);

    if (pThread) {
        pThread->Run();
    }

    _endthreadex(0);
    return 0;
}

}
}
