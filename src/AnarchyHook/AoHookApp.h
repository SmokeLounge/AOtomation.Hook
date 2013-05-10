#pragma once
#include "Functions.h"
#include "Common/SingletonDynamic.h"

namespace AnarchyHook {

namespace Communication {
class IpcClient;
class IpcServer;
}

using namespace std;

using namespace AnarchyHook::Communication;

class AoHookApp : public Common::SingletonDynamic<AoHookApp> {
public:
    AoHookApp();
    virtual ~AoHookApp();
    void Start();
    void Stop();
private:
    typedef shared_ptr<IpcClient> IpcClientPtr;
    typedef shared_ptr<IpcServer> IpcServerPtr;

    EmptyClass* connection;
    IpcClientPtr connectionSendClient;
    IpcClientPtr dataBlockToMessageClient;
    IpcServerPtr ipcServer;
    Connection_t_Send originalConnectionSend;

    LONG FindAndAttach(LPCSTR pszModule, LPCSTR pszFunction,  PVOID* ppPointer, PVOID pDetour);
    void OnConnectionSend(const void* connection, const unsigned int& size, const void* packet);
    void OnDataBlockToMessage(const unsigned int& size, const void* dataBlock);
    void OnIpcMessageReceived(const char* message, const int& size);
};

}
