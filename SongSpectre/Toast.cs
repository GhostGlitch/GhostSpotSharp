using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Gdi;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_EX_STYLE;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_STYLE;

namespace SongSpectre {
    internal class Toast : IDisposable {
        public static readonly WNDCLASSW WndClass = InitWndClass();
        public static readonly ushort atom = InitAtom(WndClass);
        public HWND hwnd;

        private bool disposed = false;
        private static readonly Dictionary<HWND, Toast> _Instances = [];

        static  unsafe WNDCLASSW InitWndClass() {
            nint ClassNameP = Marshal.StringToHGlobalUni("SpectralToast");
            WNDCLASSW win = new() {
                lpszClassName = new PCWSTR((char*)ClassNameP),
                lpfnWndProc = SpecWinProc,
                hInstance = (HINSTANCE)PI.GetModuleHandle((string?)null).DangerousGetHandle(),
                hbrBackground = PI.GetSysColorBrush(SYS_COLOR_INDEX.COLOR_BACKGROUND),
                hCursor = HCURSOR.Null
            };
            Marshal.FreeHGlobal(ClassNameP);
            return win;
        }
        static unsafe ushort InitAtom(WNDCLASSW win) {
            return PI.RegisterClass(win);
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
        private static LRESULT SpecWinProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam) {
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (hwnd != HWND.Null) {
                    PI.DestroyWindow(hwnd);
                    hwnd = HWND.Null;
                }
                _Instances.Remove(hwnd);
                disposed = true;
            }
        }
        ~Toast() {   
                Dispose(false);
        }
    }
}