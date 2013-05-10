#include "StdAfx.h"
#include "Mutex.h"

namespace AnarchyHook {
namespace Threading {

Mutex::Mutex() {
    InitializeCriticalSection(&m_mutex);
}

Mutex::~Mutex() {
    DeleteCriticalSection(&m_mutex);
}

void Mutex::Acquire() {
    EnterCriticalSection(&m_mutex);
}

void Mutex::Release() {
    LeaveCriticalSection(&m_mutex);
}

}
}
