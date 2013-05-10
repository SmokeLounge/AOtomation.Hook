#include "stdafx.h"
#include "Lock.h"

namespace AnarchyHook {
namespace Threading {

Lock::Lock(Mutex &mutex) : m_mutex(mutex) {
    m_mutex.Acquire();
}

Lock::~Lock() {
    m_mutex.Release();
}

}
}
