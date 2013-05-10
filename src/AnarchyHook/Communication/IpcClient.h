#pragma once
#include <string>

namespace AnarchyHook {
namespace Communication {

using namespace std;

class IpcClient {
public:
    IpcClient(string pipeName);
    virtual ~IpcClient(void);
    bool IsConnected() const;
    void Connect();
    void Disconnect();
    void Send(const void* message, const int& size);
protected:
    typedef shared_ptr<BYTE> BufferPtr;

    BufferPtr inBuffer;
    DWORD inBufferSize;
    HANDLE pipeHandle;
    string pipeName;
    HANDLE CreatePipe();
};

}
}
