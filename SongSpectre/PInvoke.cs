namespace SongSpectre {
    internal static class PInvoke {
        [DllImport("uxtheme.dll", EntryPoint = "#95", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint GetImmersiveColorFromColorSetEx(
        uint dwImmersiveColorSet, uint dwImmersiveColorType,
        bool bIgnoreHighContrast, uint dwHighContrastCacheMode);
        [DllImport("uxtheme.dll", EntryPoint = "#96", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint GetImmersiveColorTypeFromName(IntPtr pName);
        [DllImport("uxtheme.dll", EntryPoint = "#98", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetImmersiveUserColorSetPreference(
        bool bForceCheckRegistry, bool bSkipCheckOnFail);
    }
}