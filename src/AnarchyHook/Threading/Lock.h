#pragma once
#include "Mutex.h"

namespace AnarchyHook {
namespace Threading {

// Simple Win32 Thread Class - David Poon
// http://www.flipcode.com/archives/Simple_Win32_Thread_Class.shtml

class Lock {
public:
    Lock(Mutex &mutex);
    ~Lock();

private:
    Mutex &m_mutex;
};

}
}
