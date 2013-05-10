#include "stdafx.h"
#include "Thread.h"

namespace AnarchyHook {
namespace Threading {

Thread::Thread(ThreadStart threadStart) :
    threadStart(threadStart) {}

Thread::~Thread() {}

void Thread::Run() {
    if (this->threadStart) {
        this->threadStart();
    }
}

}
}
