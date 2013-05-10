#pragma once
#include <string>

namespace AnarchyHook {

namespace Threading {
class Thread;
}

namespace Communication {

using namespace std;

using namespace AnarchyHook::Threading;

typedef std::function<void (const char* message, const int& size)> ReceiveCallback;

class IpcServer {
public:
    IpcServer(string pipeName, ReceiveCallback);
    virtual ~IpcServer(void);

    bool IsRunning() const;
    string PipeName() const;

    void Start();
    void Stop();
private:
    typedef shared_ptr<Thread> ThreadPtr;

    bool isRunning;
    DWORD inBufferSize;
    DWORD outBufferSize;
    HANDLE pipeHandle;
    string pipeName;
    ReceiveCallback receivedCallback;
    ThreadPtr workerThread;

    HANDLE CreatePipe();
    void OnReceived(const char* message, const int& size);
    void Run();
};

}
}
