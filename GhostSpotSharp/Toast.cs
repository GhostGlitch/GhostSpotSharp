﻿using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Gdi;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_EX_STYLE;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_STYLE;

namespace GhostSpotSharp {
    internal class Toast : IDisposable {
        public static readonly ushort atom;
        public static readonly WNDCLASSW WndClass;
        public HWND hwnd;

        private bool disposed = false;
        private static readonly Dictionary<HWND, Toast> _Instances = [];

        static unsafe Toast() {
            nint ClassNameP = Marshal.StringToHGlobalUni("GhosToast");
            WNDCLASSW win = new() {
                lpszClassName = new PCWSTR((char*)ClassNameP),
                lpfnWndProc = GhostWinProc,
                hInstance = (HINSTANCE)PI.GetModuleHandle((string?)null).DangerousGetHandle(),
                hbrBackground = PI.GetSysColorBrush(SYS_COLOR_INDEX.COLOR_BACKGROUND),
                hCursor = HCURSOR.Null
            };
            WndClass = win;
            atom = PI.RegisterClass(win);
            Marshal.FreeHGlobal(ClassNameP);
        }
        public unsafe Toast() {
            nint TitleP = Marshal.StringToHGlobalUni("title");
            hwnd = PI.CreateWindowEx(
                WS_EX_TOPMOST | WS_EX_TRANSPARENT| 
                WS_EX_LAYERED | WS_EX_NOACTIVATE,
                WndClass.lpszClassName,
                new PCWSTR((char*)TitleP),
                WS_POPUP,
                PI.CW_USEDEFAULT, PI.CW_USEDEFAULT,
                300, 200,
                HWND.Null, HMENU.Null,
                WndClass.hInstance );
            Marshal.FreeHGlobal(TitleP);
            _Instances[hwnd] = this;
        }
        private static LRESULT GhostWinProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam) {
            switch (msg) {
                case PI.WM_CLOSE:
                    if (_Instances.TryGetValue(hWnd, out Toast? toast)) {
                        toast.Dispose();
                        return (LRESULT)0;
                    } else {
                        return (LRESULT)1;
                    }
                default:
                    return PI.DefWindowProc(hWnd, msg, wParam, lParam);
            }
        }
        public void Dispose() {
            if (hwnd != HWND.Null) {
                PI.DestroyWindow(hwnd);
                hwnd = HWND.Null;
            }
            _Instances.Remove(hwnd);
            disposed = true;
            GC.SuppressFinalize(this);
        }
        ~Toast() {
            Dispose();
        }
    }
}