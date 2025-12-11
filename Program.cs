using System;
using DisplaySwitch.Services;
using System.Linq;

namespace DisplaySwitch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layoutServices = new LayoutServices();
            var snapshot = layoutServices.CaptureCurrentLayout();

            if (snapshot.Primary == null)
            {
                Console.WriteLine("Kein Primaermonitor gefunden.");
                return;
            }

            Console.WriteLine($"Primaer: {snapshot.Primary}");
            if (snapshot.Secondaries.Count == 0)
            {
                Console.WriteLine("Kein sekundaerer Monitor erkannt.");
                return;
            }

            Console.WriteLine("Sekundaer(e):");
            foreach (var secondary in snapshot.Secondaries)
            {
                Console.WriteLine($" - {secondary}");
            }

            var firstSecondary = snapshot.Secondaries.First();
            bool wasLeftOfPrimary = firstSecondary.PositionX < snapshot.Primary.PositionX;

            bool moved = layoutServices.ToggleSecondarySide(snapshot);
            if (!moved)
            {
                Console.WriteLine("Verschieben fehlgeschlagen.");
                return;
            }

            Console.WriteLine(wasLeftOfPrimary
                ? "Sekundaerer Monitor wurde nach rechts verschoben."
                : "Sekundaerer Monitor wurde nach links verschoben.");
        }
    }
}
