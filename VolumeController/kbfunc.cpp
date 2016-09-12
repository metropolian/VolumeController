#include "stdafx.h"
#include "kbfunc.h"

HHOOK SetCaptureKeyboard(HINSTANCE HInst, HOOKPROC KeyboardProcLL)
{
	return SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardProcLL, HInst, 0);
}

bool SetReleaseKeyboard(HHOOK Handle)
{
	return UnhookWindowsHookEx(Handle);
}
