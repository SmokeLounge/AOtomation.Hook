#pragma once
#include <functional>

#include "Win32Thread.h"

namespace AnarchyHook {
namespace Threading {

typedef std::function<void ()> ThreadStart;

class Thread : public Win32Thread {
public:
    Thread(ThreadStart threadStart);
    virtual ~Thread();
private:
    ThreadStart threadStart;

    virtual void Run();
};

}
}
