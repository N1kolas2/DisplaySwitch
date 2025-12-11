using System;
using System.Runtime.InteropServices;

namespace DisplaySwitch
{
    public static class NativeMethods
    {
        // ================================
        // 1. KONSTANTEN
        // ================================
        public const string User32 = "user32.dll";

        public const int DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001;
        public const int DISPLAY_DEVICE_PRIMARY_DEVICE      = 0x00000004;

        public const int ENUM_CURRENT_SETTINGS = -1;

        // ChangeDisplaySettingsEx flags/result codes
        public const int CDS_UPDATEREGISTRY = 0x00000001;
        public const int CDS_NORESET = 0x10000000;
        public const int DISP_CHANGE_SUCCESSFUL = 0;

        // dmFields bits (partial)
        [Flags]
        public enum DM : int
        {
            Position = 0x00000020,
        }


        // ================================
        // 2. DISPLAY_DEVICE STRUCT
        // ================================
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DISPLAY_DEVICE
        {
            public int cb;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;

            public int StateFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }


        // ================================
        // 3. DEVMODE STRUCT
        // (nur relevante Felder)
        // ================================
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;

            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;

            public int dmPositionX;
            public int dmPositionY;

            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;

            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;

            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;

            // Restliche Felder brauchst du aktuell NICHT
        }


        // ================================
        // 4. P/INVOKE: EnumDisplayDevices
        // ================================
        [DllImport(User32, CharSet = CharSet.Unicode)]
        public static extern bool EnumDisplayDevices(
            string? lpDevice,               // null = alle Display-Adapter durchgehen
            uint iDevNum,                  // welcher Index
            ref DISPLAY_DEVICE lpDisplayDevice,
            uint dwFlags
        );


        // ================================
        // 5. P/INVOKE: EnumDisplaySettings
        // ================================
        [DllImport(User32, CharSet = CharSet.Unicode)]
        public static extern bool EnumDisplaySettings(
            string lpszDeviceName,
            int iModeNum,
            ref DEVMODE lpDevMode
        );

        // ================================
        // 6. P/INVOKE: ChangeDisplaySettingsEx
        // ================================
        [DllImport(User32, CharSet = CharSet.Unicode)]
        public static extern int ChangeDisplaySettingsEx(
            string? lpszDeviceName,
            ref DEVMODE lpDevMode,
            IntPtr hwnd,
            int dwflags,
            IntPtr lParam
        );

        [DllImport(User32, CharSet = CharSet.Unicode)]
        public static extern int ChangeDisplaySettingsEx(
            string? lpszDeviceName,
            IntPtr lpDevMode,
            IntPtr hwnd,
            int dwflags,
            IntPtr lParam
        );
    }
}
