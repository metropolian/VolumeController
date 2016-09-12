#include "stdafx.h"
#include "defines.h"

HWND SysCreateWindow(WNDPROC CurWindowProc, DWORD Styles, DWORD ClassStyle,
	LPTSTR WinName, LPTSTR Title,
	DWORD Left, DWORD Top,
	DWORD Width, DWORD Height,
	HICON HIcon,
	bool Show,
	bool NewClass)
{
	WNDCLASS WinClass;
	HANDLE HInst = GetModuleHandle(0);
	HWND HWin = 0;

	if (Styles == 0)
		Styles = WS_EX_APPWINDOW;

	if (ClassStyle == 0)
		ClassStyle = CS_DBLCLKS | CS_OWNDC | CS_HREDRAW | CS_VREDRAW;

	WinClass.style = ClassStyle;
	WinClass.lpfnWndProc = CurWindowProc;
	WinClass.cbClsExtra = 0;
	WinClass.cbWndExtra = 0;
	WinClass.hInstance = (HINSTANCE)HInst;
	WinClass.hIcon = HIcon;
	WinClass.hCursor = LoadCursor(0, IDI_APPLICATION);
	WinClass.hbrBackground = GetSysColorBrush(COLOR_BTNFACE); // CreateSolidBrush( 0x000000 );
	WinClass.lpszMenuName = NULL;
	WinClass.lpszClassName = WinName;

	if (!RegisterClass(&WinClass))
		return 0;

	HWin = CreateWindowEx(Styles, WinName, Title,
		WS_OVERLAPPEDWINDOW | (Show ? WS_VISIBLE : 0),
		Left, Top, Width, Height,
		NULL, NULL, (HINSTANCE)HInst, NULL);

	if (HWin && Show)
	{
		ShowWindow((HWND)HWin, SW_NORMAL);
	}

	return HWin;
}

bool SysHandleMessage(HWND HWin)
{
	if (HWin)
	{
		MSG CurMsg;

		DWORD Res = PeekMessage(&CurMsg, 0, 0, 0, PM_REMOVE);

		if ((CurMsg.message == WM_QUIT) ||
			(CurMsg.message == WM_DESTROY))
			return false;

		if (Res)
		{
			TranslateMessage(&CurMsg);
			DispatchMessage(&CurMsg);
		}
		return true;
	}

	return false;
}

/* Execute a program */
DWORD AppExecute(LPTSTR FName, LPTSTR Param, bool Wait)
{
	STARTUPINFO StInfo = { sizeof(StInfo) };
	PROCESS_INFORMATION ProcInfo;
	DWORD Res = 0;

	if (CreateProcess(FName, NULL, NULL, NULL, true, 0, NULL, NULL, &StInfo, &ProcInfo))
	{
		if (Wait)
			WaitForSingleObject(ProcInfo.hProcess, INFINITE);
		GetExitCodeProcess(ProcInfo.hProcess, &Res);
		return 1 + Res;
	}
	return 0;
}

