using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
namespace SongSpectre {
    internal static class UIHelpers {
        public static HBRUSH ColorToBrush(UiColor color) {
            return PI.CreateSolidBrush(MakeColorRef(color.R, color.G, color.B));
        }
        public static HBRUSH ColorToBrush(uint ARGBColor) {
            byte r = (byte)((ARGBColor >> 16) & 0xFF);
            byte g = (byte)((ARGBColor >> 8) & 0xFF);
            byte b = (byte)(ARGBColor & 0xFF);
            return PI.CreateSolidBrush(MakeColorRef(r, g, b));
        }
        public static HBRUSH ColorToBrush(byte r, byte g, byte b) {
            return PI.CreateSolidBrush(MakeColorRef(r, g, b));
        }
        public static COLORREF MakeColorRef(byte r, byte g, byte b) {
            return (COLORREF)((uint)(r | (g << 8) | (b << 16)));
        }
    }
}