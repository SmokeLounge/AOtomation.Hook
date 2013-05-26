#include "stdafx.h"
#include "AoHookApp.h"

#include <functional>
#include <string>
#include <sstream>

#include "Hooks.h"
#include "Communication/IpcClient.h"
#include "Communication/IpcServer.h"

namespace AnarchyHook {

using namespace std;
using namespace std::placeholders;

AoHookApp::AoHookApp() :
    connection(nullptr),
    connectionSendClient(nullptr),
    dataBlockToMessageClient(nullptr),
    ipcServer(nullptr),
    originalConnectionSend(nullptr) {
    auto onConnectionSend = bind(&AoHookApp::OnConnectionSend, this, _1, _2, _3);
    Hooks::SetConnectionSendCallback(onConnectionSend);
    auto onDataBlockToMessage = std::bind(&AoHookApp::OnDataBlockToMessage, this, _1, _2);
    Hooks::SetDataBlockToMessageCallback(onDataBlockToMessage);

    auto processId = GetCurrentProcessId();
    std::stringstream stringstream(stringstream::in | stringstream::out);
    stringstream << "AnarchyHook" << dec << processId;
    auto pipeName = stringstream.str();

    auto receiveCallback = bind(&AoHookApp::OnIpcMessageReceived, this, _1, _2);
    this->ipcServer = IpcServerPtr(new IpcServer(pipeName, receiveCallback));
    this->connectionSendClient = IpcClientPtr(new IpcClient(pipeName + "cs"));
    this->dataBlockToMessageClient = IpcClientPtr(new IpcClient(pipeName + "dm"));
}

AoHookApp::~AoHookApp() {}

void AoHookApp::Start() {
    Hooks::AttachHooks();
    this->originalConnectionSend = Hooks::GetOriginalConnectionSend();
    this->ipcServer->Start();
}

void AoHookApp::Stop() {
    this->ipcServer->Stop();
    this->connectionSendClient->Disconnect();
    this->dataBlockToMessageClient->Disconnect();
    this->originalConnectionSend = nullptr;
    Hooks::DetachHooks();
}

void AoHookApp::OnConnectionSend(const void* connection, const unsigned int& size, const void* packet) {
    this->connection = reinterpret_cast<EmptyClass*>(const_cast<void*>(connection));
    if (this->connectionSendClient->IsConnected() == false) {
        return;
    }

    this->connectionSendClient->Send(packet, size);
}

void AoHookApp::OnDataBlockToMessage(const unsigned int& size, const void* dataBlock) {
    if (this->dataBlockToMessageClient->IsConnected() == false) {
        return;
    }

	this->dataBlockToMessageClient->Send(dataBlock, size);
}

void AoHookApp::OnIpcMessageReceived(const char* message, const int& size) {
    string messageStr = string(message).substr(0, 4);
    if (messageStr == "hai!") {
        this->connectionSendClient->Connect();
        this->dataBlockToMessageClient->Connect();
        return;
    }

    if (messageStr == "stop") {
        this->Stop();
        return;
    }

    if (this->connection != nullptr) {
        (this->connection->*(this->originalConnectionSend))(0, size, message);
    }
}

}
