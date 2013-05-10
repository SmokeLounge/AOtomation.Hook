// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <windows.h>

#include "AoHookApp.h"

using namespace std;

using namespace AnarchyHook;

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    (void)hModule;
    (void)lpReserved;

    if (ul_reason_for_call == DLL_PROCESS_ATTACH) {
        AoHookApp::instance().Start();
    } else if (ul_reason_for_call == DLL_PROCESS_DETACH) {
        AoHookApp::instance().Stop();
    }

    return TRUE;
}

