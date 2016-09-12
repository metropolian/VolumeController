// stdafx.h : include file for standard system include files,
//      or project specific include files that are used frequently,
//      but are changed infrequently

#pragma once

// Windows Platform headers and control defines
#define STRICT
#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
#define _WIN32_WINNT 0x0500	// Change this to the appropriate value to target other versions of Windows.
#define _WIN32_WINDOWS 0x0500 // Change this to the appropriate value to target Windows Me or later.
#define _WIN32_IE 0x0500	// Change this to the appropriate value to target other versions of IE.

#include <windows.h>
#include <shellapi.h>

#include <cstring>
#include <stdio.h>

#include "resource.h"
#include "defines.h"
#include "kbfunc.h"
#include "sysfunc.h"

#include "Mmdeviceapi.h"
#pragma comment(lib, "uuid.lib")

using namespace std;
