using System;
using System.Linq;
using System.Runtime.InteropServices;
using DisplaySwitch.Models;

namespace DisplaySwitch.Services
{
    public class LayoutServices
    {
        public MonitorLayoutSnapshot CaptureCurrentLayout()
        {
            var snapshot = new MonitorLayoutSnapshot();
            uint deviceIndex = 0;

            while (true)
            {
                var dd = new NativeMethods.DISPLAY_DEVICE();
                dd.cb = Marshal.SizeOf<NativeMethods.DISPLAY_DEVICE>();

                if (!NativeMethods.EnumDisplayDevices(null, deviceIndex, ref dd, 0))
                    break;

                bool attached = (dd.StateFlags & NativeMethods.DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) != 0;
                if (!attached)
                {
                    deviceIndex++;
                    continue;
                }

                var mode = new NativeMethods.DEVMODE();
                mode.dmSize = (short)Marshal.SizeOf<NativeMethods.DEVMODE>();

                if (!NativeMethods.EnumDisplaySettings(dd.DeviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref mode))
                {
                    deviceIndex++;
                    continue;
                }

                var info = new MonitorLayoutInfo
                {
                    DeviceName = dd.DeviceName,
                    DeviceString = dd.DeviceString,
                    PositionX = mode.dmPositionX,
                    PositionY = mode.dmPositionY,
                    Width = mode.dmPelsWidth,
                    Height = mode.dmPelsHeight,
                    IsPrimary = (dd.StateFlags & NativeMethods.DISPLAY_DEVICE_PRIMARY_DEVICE) != 0
                };

                if (info.IsPrimary)
                    snapshot.Primary = info;
                else
                    snapshot.Secondaries.Add(info);

                deviceIndex++;
            }

            return snapshot;
        }

        public bool MoveSecondaryToRight()
        {
            var snapshot = CaptureCurrentLayout();
            return MoveSecondaryToRight(snapshot);
        }

        public bool MoveSecondaryToRight(MonitorLayoutSnapshot snapshot)
        {
            if (snapshot.Primary == null)
                return false;
            if (snapshot.Secondaries.Count == 0)
                return false;

            var primary = snapshot.Primary;
            var secondary = snapshot.Secondaries.First();

            int newX = primary.PositionX + primary.Width;
            int newY = primary.PositionY;

            bool updated = ApplyPosition(secondary.DeviceName, newX, newY);
            if (!updated)
                return false;

            NativeMethods.ChangeDisplaySettingsEx(null, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero);
            return true;
        }

        public bool MoveSecondaryToLeft()
        {
            var snapshot = CaptureCurrentLayout();
            return MoveSecondaryToLeft(snapshot);
        }

        public bool MoveSecondaryToLeft(MonitorLayoutSnapshot snapshot)
        {
            if (snapshot.Primary == null)
                return false;
            if (snapshot.Secondaries.Count == 0)
                return false;

            var primary = snapshot.Primary;
            var secondary = snapshot.Secondaries.First();

            int newX = primary.PositionX - secondary.Width;
            int newY = primary.PositionY;

            bool updated = ApplyPosition(secondary.DeviceName, newX, newY);
            if (!updated)
                return false;

            NativeMethods.ChangeDisplaySettingsEx(null, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero);
            return true;
        }

        public bool ToggleSecondarySide()
        {
            var snapshot = CaptureCurrentLayout();
            return ToggleSecondarySide(snapshot);
        }

        public bool ToggleSecondarySide(MonitorLayoutSnapshot snapshot)
        {
            if (snapshot.Primary == null)
                return false;
            if (snapshot.Secondaries.Count == 0)
                return false;

            var primary = snapshot.Primary;
            var secondary = snapshot.Secondaries.First();

            // Treat anything with its left edge left of the primary's left edge as "left".
            bool secondaryIsLeft = secondary.PositionX < primary.PositionX;
            bool secondaryIsRight = !secondaryIsLeft;
            return secondaryIsRight
                ? MoveSecondaryToLeft(snapshot)
                : MoveSecondaryToRight(snapshot);
        }

        private static bool ApplyPosition(string deviceName, int x, int y)
        {
            var mode = new NativeMethods.DEVMODE();
            mode.dmSize = (short)Marshal.SizeOf<NativeMethods.DEVMODE>();

            if (!NativeMethods.EnumDisplaySettings(deviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref mode))
                return false;

            mode.dmPositionX = x;
            mode.dmPositionY = y;
            mode.dmFields |= (int)NativeMethods.DM.Position;

            int result = NativeMethods.ChangeDisplaySettingsEx(
                deviceName,
                ref mode,
                IntPtr.Zero,
                NativeMethods.CDS_UPDATEREGISTRY | NativeMethods.CDS_NORESET,
                IntPtr.Zero);

            return result == NativeMethods.DISP_CHANGE_SUCCESSFUL;
        }
    }
}
