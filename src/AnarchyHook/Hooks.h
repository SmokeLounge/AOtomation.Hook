#pragma once
#include <functional>

#include "Functions.h"

namespace AnarchyHook {

typedef std::function<void (const void* object, const unsigned int& size, const void* packet)> ConnectionSendCallback;
typedef std::function<void (const unsigned int& size, const void* dataBlock)> DataBlockToMessageCallback;

class Hooks {
public:
    static ConnectionSendCallback& GetConnectionSendCallback();
    static void SetConnectionSendCallback(ConnectionSendCallback val);
    static DataBlockToMessageCallback& GetDataBlockToMessageCallback();
    static void SetDataBlockToMessageCallback(DataBlockToMessageCallback val);
    static Connection_t_Send& GetOriginalConnectionSend();
    static bool AttachHooks();
    static bool DetachHooks();
private:
    Hooks();
    virtual ~Hooks();

    static Connection_t_Send orgConnection_t_Send;
    static ConnectionSendCallback connectionSendCallback;
    static DataBlockToMessage orgDataBlockToMessage;
    static DataBlockToMessageCallback dataBlockToMessageCallback;

    static LONG FindAndAttach(LPCSTR pszModule, LPCSTR pszFunction,  PVOID* ppPointer, PVOID pDetour);
    static Message_t* DataBlockToMessageHook(unsigned int size, void* dataBlock);
    static int __fastcall Connection_t_SendHook(EmptyClass *const object, int /*dummy*/, unsigned int pArg1, unsigned int pArg2, void const* pArg3);
};

}
