// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

// Exclude rarely-used stuff from Windows headers
#define WIN32_LEAN_AND_MEAN
// Windows Header Files:
#include <windows.h>

// Addiditonal Windows SDK headers
#include <process.h>

// STL headers
#include <functional>
#include <memory>
#include <sstream>
#include <string>

// 3rd party headers
#include "detver.h"
#include "detours.h"


// TODO: reference additional headers your program requires here
