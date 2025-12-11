using System;

namespace DisplaySwitch.Models
{
    public class MonitorLayoutInfo
    {
        public string DeviceName { get; init; } = string.Empty;
        public string DeviceString { get; init; } = string.Empty;
        public int PositionX { get; init; }
        public int PositionY { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public bool IsPrimary { get; init; }

        public override string ToString()
        {
            return $"{DeviceString} ({DeviceName}) @ ({PositionX},{PositionY}) {Width}x{Height} primary={IsPrimary}";
        }
    }
}
