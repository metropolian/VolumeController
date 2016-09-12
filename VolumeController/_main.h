/* Global Variables */
#define APP_TITLE		"DMLFX_K2M"
#define APP_INFO		"Mouse To Keyboard for Accesibility Utility"
#define WM_SHELLNOTIFY	WM_APP + 400
#define WC_MENU_EXIT	1200
#define WC_MENU_CONFIG	1201
#define WC_MENU_ABOUT 	1202

HINSTANCE HInst;
HMENU HDefMenu;
HWND HWin;
HICON HIcon;

LRESULT CALLBACK WndProc(HWND HWin, UINT Message, WPARAM WParam, LPARAM LParam);