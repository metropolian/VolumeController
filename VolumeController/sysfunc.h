#pragma once

HWND SysCreateWindow(WNDPROC CurWindowProc, DWORD Styles, DWORD ClassStyle,
	LPTSTR WinName, LPTSTR Title,
	DWORD Left, DWORD Top,
	DWORD Width, DWORD Height,
	HICON HIcon,
	bool Show,
	bool NewClass);

bool SysHandleMessage(HWND HWin);

bool Shell_AddIcon(HWND HWin, HICON HIcon, DWORD Msg, LPTSTR Tip);

bool Shell_DelIcon();

LPTSTR ReadStrConfig(LPTSTR Fname, LPTSTR FSection, LPTSTR Key, LPTSTR Def);

int ReadIntConfig(LPTSTR Fname, LPTSTR FSection, LPTSTR Key, int Def);
