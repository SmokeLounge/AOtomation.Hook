#pragma once
#include <winbase.h>

namespace AnarchyHook {
namespace Threading {

// Simple Win32 Thread Class - David Poon
// http://www.flipcode.com/archives/Simple_Win32_Thread_Class.shtml

class Mutex {
public:
    Mutex();
    ~Mutex();
    void Acquire();
    void Release();
private:
    CRITICAL_SECTION m_mutex;
};

}
}
