#pragma once
#include "Mutex.h"

namespace AnarchyHook {
namespace Threading {

// Simple Win32 Thread Class - David Poon
// http://www.flipcode.com/archives/Simple_Win32_Thread_Class.shtml

class Win32Thread {
public:
    Win32Thread();
    virtual ~Win32Thread();
    unsigned int Id() const;
    bool Create(unsigned int stackSize = 0);
    void Start();
    void Join();
    void Resume();
    void Suspend();
    void Shutdown();
protected:
    bool CanRun();
    virtual void Run() = 0;
private:
    HANDLE threadHandle;
    unsigned int id;
    volatile bool canRun;
    volatile bool suspended;
    Mutex mutex;
    static unsigned int __stdcall threadFunc(void *args);
};

}
}
