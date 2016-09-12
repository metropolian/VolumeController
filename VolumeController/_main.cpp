/* -------------------------------------------------------------------------------------
* MAIN.CPP - Mouse To Keyboard Application
*
* DML - Kobchai petchin - Latest Date 12-10-2007
* ------------------------------------------------------------------------------------- */

#include "stdafx.h"
#include "_main.h"

DEF_STRING(FConfig)
DEF_STRING(FConfigSection)
DEF_STRING(StrTitle)
DEF_STRING(StrClass)
DEF_STRING(StrTips)
DEF_STRING(StrToConfig)
DEF_STRING(StrToAbout)
DEF_STRING(StrToExit)

const CLSID CLSID_MMDeviceEnumerator = __uuidof(MMDeviceEnumerator);
const IID IID_IMMDeviceEnumerator = __uuidof(IMMDeviceEnumerator);
#define SAFE_RELEASE(punk)  \
              if ((punk) != NULL)  \
                { (punk)->Release(); (punk) = NULL; }
void Test()
{
	HRESULT hr = S_OK;
	IMMDeviceEnumerator *pEnumerator = NULL;
	IMMDeviceCollection *pCollection = NULL;
	IMMDevice *pEndpoint = NULL;
	IPropertyStore *pProps = NULL;
	LPWSTR pwszID = NULL;

	IMMAudioSessionManager

	printf("Test");
	CoInitialize(NULL);

	hr = CoCreateInstance(
		CLSID_MMDeviceEnumerator, NULL,
		CLSCTX_ALL, IID_IMMDeviceEnumerator,
		(void**)&pEnumerator);
	

		hr = pEnumerator->EnumAudioEndpoints(
			eRender, DEVICE_STATE_ACTIVE,
			&pCollection);

	UINT  count;
	hr = pCollection->GetCount(&count);
	if (count == 0)
	{
		printf("No endpoints found.\n");
	}

	// Each loop prints the name of an endpoint device.
	for (ULONG i = 0; i < count; i++)
	{
		// Get pointer to endpoint number i.
		hr = pCollection->Item(i, &pEndpoint);

		// Get the endpoint ID string.
		hr = pEndpoint->GetId(&pwszID);
		hr = pEndpoint->OpenPropertyStore(
				STGM_READ, &pProps);


		static PROPERTYKEY PKEY_Device_FriendlyName;
		GUID IDevice_FriendlyName = { 0xa45c254e, 0xdf1c, 0x4efd,{ 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0 } };
		PKEY_Device_FriendlyName.pid = 14;
		PKEY_Device_FriendlyName.fmtid = IDevice_FriendlyName;
		PROPVARIANT varName;
		// Initialize container for property value.
		PropVariantInit(&varName);

		
		// Get the endpoint's friendly-name property.
		hr = pProps->GetValue(
			PKEY_Device_FriendlyName, &varName);
			// Print endpoint friendly name and endpoint ID.
			printf("Endpoint %d: \"%S\" (%S)\n",
				i, varName.pwszVal, pwszID);

		CoTaskMemFree(pwszID);
		pwszID = NULL;
		PropVariantClear(&varName);
		SAFE_RELEASE(pProps);
		SAFE_RELEASE(pEndpoint);
	}
	SAFE_RELEASE(pEnumerator);
	SAFE_RELEASE(pCollection);
}

/* -------------------------------------------------------------------------------------
* SETUP
* ------------------------------------------------------------------------------------- */
bool Setup()
{
	Test();
	return 0;

	DEF_STRING(Res);
	DEF_STRING(Val);
	LPTSTR PName = NULL;

	GetModuleFileName(GetModuleHandle(0), Res, 256);
	GetFullPathName(Res, 256, Val, &PName);
	if (PName)
		PName[0] = 0;
	return SetCurrentDirectory(Val);
}

/* -------------------------------------------------------------------------------------
* KEYBOARD PROCEDURE EVENTS
* ------------------------------------------------------------------------------------- */
HHOOK HKeyHook;
float CursorVelo = 1;
float CursorSpeed = 1;
float CursorAccel = 1;
bool DisableKeys = false;

struct KEYCUSTOM_INFO
{
	DWORD Up, Left, Right, Down;
	DWORD UpLeft, UpRight, DownLeft, DownRight;
	DWORD LClick, MClick, RClick;
	DWORD Quit;

} KeyCustom, KeyState;

Keyboard_Procedure()
{
	DWORD KeyCode;
	GetKeyInput_Struct(KB);

	if (NCode == HC_ACTION)
		switch (WParam)
		{
		case WM_KEYDOWN:

			
			KeyCode = KB->vkCode;
			
			if (KeyCode == KeyCustom.Quit) /* Quit Key */
			{
				PostQuitMessage(0);
				return 1;
			}
			break;
		case WM_KEYUP:
			break;
		case WM_SYSKEYDOWN:
		case WM_SYSKEYUP:
			break;
		}

	if (DisableKeys)
		return 1;

	return CallNextHookEx(0, NCode, WParam, LParam);
}




bool ReloadConfig()
{
	SET_STRING(StrClass, "");

	SET_STRING(FConfig, "./CONFIG.INI");
	SET_STRING(FConfigSection, "Application");

	SET_STRING(StrTitle, ReadStrConfig(FConfig, FConfigSection, "Title", "DMLFX Key To Mouse Accessibility"));
	SET_STRING(StrClass, ReadStrConfig(FConfig, FConfigSection, "Class", "DMLFX"));
	SET_STRING(StrTips, ReadStrConfig(FConfig, FConfigSection, "Tips", "Key To Mouse (Menu)"));
	SET_STRING(StrToConfig, ReadStrConfig(FConfig, FConfigSection, "NameConfig", "Configuration"));
	SET_STRING(StrToAbout, ReadStrConfig(FConfig, FConfigSection, "NameAbout", "About"));
	SET_STRING(StrToExit, ReadStrConfig(FConfig, FConfigSection, "NameExit", "Quit"));

	if (LEN_STRING(StrClass) < 1)
		return false;

	SET_STRING(FConfigSection, "Key_Customized");

	/*KeyCustom.Up = ReadIntConfig("C_Up", VK_NUMPAD8);
	KeyCustom.UpLeft = ReadIntConfig("C_UpLeft", VK_NUMPAD7);
	KeyCustom.UpRight = ReadIntConfig("C_UpRight", VK_NUMPAD9);

	KeyCustom.Left = ReadIntConfig("C_Left", VK_NUMPAD4);
	KeyCustom.Right = ReadIntConfig("C_Right", VK_NUMPAD6);

	KeyCustom.Down = ReadIntConfig("C_Down", VK_NUMPAD2);
	KeyCustom.DownLeft = ReadIntConfig("C_DownLeft", VK_NUMPAD1);
	KeyCustom.DownRight = ReadIntConfig("C_DownRight", VK_NUMPAD3);

	KeyCustom.LClick = ReadIntConfig("C_LClick", VK_NUMPAD0);
	KeyCustom.MClick = ReadIntConfig("C_MClick", VK_SUBTRACT);
	KeyCustom.RClick = ReadIntConfig("C_RClick", VK_ADD); */

	return true;
}

#define RUN_AppExecute_KeyConfig AppExecute("KeyConfig.exe", "", true )
#define RUN_AppExecute_About AppExecute("About.exe", "", false )

/* -------------------------------------------------------------------------------------
* WINMAIN
* ------------------------------------------------------------------------------------- */
int APIENTRY WinMain(HINSTANCE hInstance,
	HINSTANCE hPrevInstance,
	LPTSTR    lpCmdLine,
	int       nCmdShow)
{
	Setup();
	/* Loading Configuration */
	if (!ReloadConfig())
	{
		RUN_AppExecute_KeyConfig;
		if (!ReloadConfig())
		{
			MessageBox(NULL, "Failed to Start, Please Reinstall Application.", APP_TITLE, MB_OK | MB_ICONERROR);
			return 255;
		}
	} 

	/* Init Windows */
	HInst = hInstance;
	HIcon = LoadIcon(HInst, (LPTSTR)IDI_DEFAULT);
	HWND HWin = SysCreateWindow(WndProc, 0, 0, StrClass, StrTitle, 0, 0, 640, 480, HIcon, false, true);
	//	ShowWindow( HWin, SW_MINIMIZE);

	/* Create Tray Menu */
	HDefMenu = CreatePopupMenu();
	AppendMenu(HDefMenu, MF_STRING, 0, StrTitle);
	AppendMenu(HDefMenu, MF_SEPARATOR, 0, NULL);
	AppendMenu(HDefMenu, MF_STRING, WC_MENU_CONFIG, StrToConfig);
	AppendMenu(HDefMenu, MF_STRING, WC_MENU_ABOUT, StrToAbout);
	AppendMenu(HDefMenu, MF_SEPARATOR, 0, NULL);
	AppendMenu(HDefMenu, MF_STRING, WC_MENU_EXIT, StrToExit);
	Shell_AddIcon(HWin, HIcon, WM_SHELLNOTIFY, StrTips);

	/* Start Capture */
	HKeyHook = SetCaptureKeyboard(HInst, KeyboardProcLL);

	/* Application Run */
	while (SysHandleMessage(HWin));

	/* Restoration */
	SetReleaseKeyboard(HKeyHook);
	Shell_DelIcon();

	return (int)0;
}


int main() 
{
	return WinMain(GetModuleHandle(NULL), NULL, GetCommandLine(), 1);
}


//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND HWin, UINT Message, WPARAM WParam, LPARAM LParam)
{
	switch (Message)
	{
	case WM_COMMAND:
		switch (WParam)
		{
		case WC_MENU_CONFIG:
		{
			if (RUN_AppExecute_KeyConfig > 1) // Call KeyConfig.exe
			{
				//SetReleaseKeyboard();
				ReloadConfig();
				//SetCaptureKeyboard();
			}
		}
		break;

		case WC_MENU_ABOUT:
		{
			RUN_AppExecute_About;
		}
		break;

		case WC_MENU_EXIT:
			PostQuitMessage(0);
			break;
		}
		return 1;

	case WM_MENUCOMMAND:
		return 1;

	case WM_PAINT:
	{
		PAINTSTRUCT ps;
		HDC hdc = BeginPaint(HWin, &ps);
		// TODO: Add any drawing code here...
		EndPaint(HWin, &ps);
	}
	return 1;

	case WM_DESTROY:
		PostQuitMessage(0);
		return 1;

	case WM_SHELLNOTIFY:
		switch (LParam)
		{
		case WM_LBUTTONUP:
		case WM_MBUTTONUP:
		case WM_RBUTTONUP:
		{
			POINT Pos;
			GetCursorPos(&Pos);
			TrackPopupMenu(HDefMenu, 0, Pos.x, Pos.y, 0, HWin, NULL);
		}
		return 1;
		}
		break;

	}
	return DefWindowProc(HWin, Message, WParam, LParam);
}
