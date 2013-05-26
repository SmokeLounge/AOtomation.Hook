#pragma once

#ifdef ANARCHYHOOK_EXPORTS
#define ANARCHYHOOK_API __declspec(dllexport)
#else
#define ANARCHYHOOK_API __declspec(dllimport)
#endif