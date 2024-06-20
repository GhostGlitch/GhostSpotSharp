using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Gdi;
using Windows.UI.ViewManagement;
using static Windows.Win32.UI.WindowsAndMessaging.LAYERED_WINDOW_ATTRIBUTES_FLAGS;
using static Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD;




namespace SongSpectre {

    internal unsafe class Test {
        private static (HBRUSH, HBRUSH) GColors() {
            UISettings UISet = new();
            UiColor UiColorBack = UISet.GetColorValue(UIColorType.Background);
            uint ImerStartBack = SI.GetImmersiveColorFromColorSetEx(
            (uint)SI.GetImmersiveUserColorSetPreference(false, false),
            SI.GetImmersiveColorTypeFromName(Marshal.StringToHGlobalUni("StartBackground")),
            false, 0);
            return (UIHelpers.ColorToBrush(UiColorBack), UIHelpers.ColorToBrush(ImerStartBack));
        }

        public static void Hope() {
            Toast tst = new();
            WriteLine(tst.hwnd);
            PI.ShowWindow(tst.hwnd, SW_SHOW);
            PI.UpdateWindow(tst.hwnd);
            PI.SetLayeredWindowAttributes(tst.hwnd, UIHelpers.MakeColorRef(126, 126, 126), 255, LWA_ALPHA);
            while (PI.GetMessage(out MSG msg, HWND.Null, 0, 0)) {
                PI.TranslateMessage(&msg);
                PI.DispatchMessage(&msg);
            }
        }
    }
}
