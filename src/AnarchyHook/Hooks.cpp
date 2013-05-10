#include "stdafx.h"
#include "Hooks.h"

#include "detours.h"

namespace AnarchyHook {

ConnectionSendCallback Hooks::connectionSendCallback = nullptr;
Connection_t_Send Hooks::orgConnection_t_Send = nullptr;
DataBlockToMessageCallback Hooks::dataBlockToMessageCallback = nullptr;
DataBlockToMessage Hooks::orgDataBlockToMessage = nullptr;

Hooks::Hooks() {}

Hooks::~Hooks() {}

ConnectionSendCallback& Hooks::GetConnectionSendCallback() {
    return Hooks::connectionSendCallback;
}

void Hooks::SetConnectionSendCallback(ConnectionSendCallback val) {
    Hooks::connectionSendCallback = val;
}

DataBlockToMessageCallback& Hooks::GetDataBlockToMessageCallback() {
    return Hooks::dataBlockToMessageCallback;
}

void Hooks::SetDataBlockToMessageCallback(DataBlockToMessageCallback val) {
    Hooks::dataBlockToMessageCallback = val;
}

Connection_t_Send& Hooks::GetOriginalConnectionSend() {
    return Hooks::orgConnection_t_Send;
}

bool Hooks::AttachHooks() {
    LONG error = 0;
    DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());
    error |= Hooks::FindAndAttach("Connection.dll", "?Send@Connection_t@@QAEHIIPBX@Z", (void**)&Hooks::orgConnection_t_Send, Hooks::Connection_t_SendHook);
    error |= Hooks::FindAndAttach("MessageProtocol.dll", "?DataBlockToMessage@@YAPAVMessage_t@@IPAX@Z", (void**)&Hooks::orgDataBlockToMessage, Hooks::DataBlockToMessageHook);
    error |= DetourTransactionCommit();
    return error == 0;
}

bool Hooks::DetachHooks() {
    LONG error = 0;
    DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());
    error |= DetourDetach((void**)&Hooks::orgConnection_t_Send, Hooks::Connection_t_SendHook);
    error |= DetourDetach((void**)&Hooks::orgDataBlockToMessage, Hooks::DataBlockToMessageHook);
    error |= DetourTransactionCommit();
    return error == 0;
}

LONG Hooks::FindAndAttach(LPCSTR pszModule, LPCSTR pszFunction,  PVOID* ppPointer, PVOID pDetour) {
    auto function = DetourFindFunction(pszModule, pszFunction);
    if (function == NULL) {
        return ERROR_INVALID_HANDLE;
    }

    *ppPointer = function;
    return DetourAttach(ppPointer, pDetour);
}

Message_t* Hooks::DataBlockToMessageHook(unsigned int size, void* dataBlock) {
    if (Hooks::dataBlockToMessageCallback != nullptr) {
        Hooks::dataBlockToMessageCallback(size, dataBlock);
    }

    return Hooks::orgDataBlockToMessage(size, dataBlock);
}

int __fastcall Hooks::Connection_t_SendHook(EmptyClass *const object, int /*dummy*/, unsigned int pArg1, unsigned int pArg2, void const* pArg3) {
    if (Hooks::connectionSendCallback != nullptr) {
        Hooks::connectionSendCallback(object, pArg2, pArg3);
    }

    return (object->*Hooks::orgConnection_t_Send) (pArg1, pArg2, pArg3);
}

}
