using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Dz2_FMA
{
    public class PlacedEvent : IJournalEntry
    {
        public char Shelf { get; }
        public int Slot { get; }
        public string ItemName { get; }

        public PlacedEvent(char shelf, int slot, string itemName)
        {
            Shelf = shelf;
            Slot = slot;
            ItemName = itemName;
        }

        public string ToLogLine() => $"{Shelf}|{Slot}|{ItemName}";

        public string ToScreenLine() => $"Размещение | полка {Shelf} | слот {Slot} | товар «{ItemName}»";

        public static PlacedEvent FromLogLine(string line)
        {
            var parts = line.Split('|');
            return new PlacedEvent(parts[0][0], int.Parse(parts[1]), parts[2]);
        }
    }
}
