using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Gdi;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_EX_STYLE;
using static Windows.Win32.UI.WindowsAndMessaging.WINDOW_STYLE;
using Windows.Win32;

namespace SongSpectre {
    internal class Toast : IDisposable {

        public static readonly WNDCLASSW WndClass;
        public static readonly ushort atom;
        private static readonly FreeLibrarySafeHandle SafeHInstance = PI.GetModuleHandle((string?)null);
        public HWND hwnd;

        private bool disposed = false;
        private static readonly Dictionary<HWND, Toast> _Instances = [];


        static unsafe Toast() {
            if (SafeHInstance.IsInvalid) {
                throw new InvalidOperationException("hInstance Handle is invalid.");
            }
            nint ClassNameP = Marshal.StringToHGlobalUni("SpectralToast");
            WNDCLASSW win = new() {
                lpszClassName = new PCWSTR((char*)ClassNameP),
                lpfnWndProc = SpecWinProc,
                hInstance = (HINSTANCE)SafeHInstance.DangerousGetHandle(),
                hbrBackground = PI.GetSysColorBrush(SYS_COLOR_INDEX.COLOR_BACKGROUND),
                hCursor = HCURSOR.Null
            };
            WndClass = win;
            atom = PI.RegisterClass(win);
            if (atom == 0) {
                throw new Exception("Window class registration failed.");
            }
            Marshal.FreeHGlobal(ClassNameP);
        }
        
        public unsafe Toast() {
            if (SafeHInstance.IsInvalid) {
                throw new InvalidOperationException("hInstance Handle is invalid.");
            }
            hwnd = PI.CreateWindowEx(
                WS_EX_TOPMOST | WS_EX_TRANSPARENT |
                WS_EX_LAYERED | WS_EX_NOACTIVATE,
                new string((char*)WndClass.lpszClassName), "title",
                WS_POPUP,
                PI.CW_USEDEFAULT, PI.CW_USEDEFAULT,
                300, 200, 
                HWND.Null, null, 
                SafeHInstance, null);
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