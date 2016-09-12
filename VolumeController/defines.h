
#ifndef _DEFINES
#define _DEFINES
/* -------------------------------------------------------------------------------------
* DEFINES.H - Default Defines
*
* DML - Kobchai petchin - Latest Date 12-10-2007
* ------------------------------------------------------------------------------------- */

#include "stdafx.h"

#define Keyboard_Procedure() LRESULT CALLBACK KeyboardProcLL(int NCode, WPARAM WParam, LPARAM LParam)

#define GetKeyInput_Struct( Var ) if (LParam == 0) return 0; KBDLLHOOKSTRUCT * Var = (KBDLLHOOKSTRUCT*)LParam;



DWORD AppExecute(LPTSTR FName, LPTSTR Param, bool Wait);

#define STRING TCHAR[256]
#define DEF_STRING(Name) TCHAR Name[256];

#ifndef UNICODE
#define SET_STRING(Name, Value) strcpy_s( Name, Value );
#define LEN_STRING(Name) strlen( Name )
#else
#define SET_STRING(Name, Value) wstrcpy( Name, Value );
#define LEN_STRING(Name) wstrlen( Name )
#endif


#endif