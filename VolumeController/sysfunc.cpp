#include "stdafx.h"
#include "sysfunc.h"

HWND SysCreateWindow(WNDPROC CurWindowProc, DWORD Styles, DWORD ClassStyle,
	LPTSTR WinName, LPTSTR Title,
	DWORD Left, DWORD Top,
	DWORD Width, DWORD Height,
	HICON HIcon,
	bool Show,
	bool NewClass);

bool SysHandleMessage(HWND HWin);

/* -------------------------------------------------------------------------------------
* SHELL TRAY ICON
* ------------------------------------------------------------------------------------- */
NOTIFYICONDATA NotifyInfo;

bool Shell_AddIcon(HWND HWin, HICON HIcon, DWORD Msg, LPTSTR Tip)
{
	NotifyInfo.cbSize = sizeof(NotifyInfo);
	NotifyInfo.hIcon = HIcon;
	NotifyInfo.hWnd = HWin;
	NotifyInfo.uFlags = NIF_ICON | NIF_TIP | NIF_MESSAGE;
	NotifyInfo.uCallbackMessage = Msg;
	SET_STRING(NotifyInfo.szTip, Tip);


	return Shell_NotifyIcon(NIM_ADD, &NotifyInfo);
}

bool Shell_DelIcon() { return Shell_NotifyIcon(NIM_DELETE, &NotifyInfo); }

/* -------------------------------------------------------------------------------------
* CONFIG
* ------------------------------------------------------------------------------------- */

LPTSTR ReadStrConfig(LPTSTR Fname, LPTSTR FSection, LPTSTR Key, LPTSTR Def)
{
	DEF_STRING(Res);
	GetPrivateProfileString(FSection, Key, Def, Res, 256, Fname);
	return Res;
}

int ReadIntConfig(LPTSTR Fname, LPTSTR FSection, LPTSTR Key, int Def)
{
	return GetPrivateProfileInt(FSection, Key, Def, Fname);;
}