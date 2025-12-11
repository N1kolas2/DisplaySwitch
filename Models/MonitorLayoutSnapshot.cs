using System.Collections.Generic;

namespace DisplaySwitch.Models
{
    public class MonitorLayoutSnapshot
    {
        public MonitorLayoutInfo? Primary { get; set; }
        public List<MonitorLayoutInfo> Secondaries { get; } = new();
    }
}
