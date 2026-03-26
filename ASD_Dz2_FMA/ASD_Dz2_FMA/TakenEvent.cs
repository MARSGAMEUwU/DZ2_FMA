using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Dz2_FMA
{
    public class TakenEvent : IJournalEntry
    {
        public char Shelf { get; }
        public int Slot { get; }
        public string ItemName { get; }

        public TakenEvent(char shelf, int slot, string itemName)
        {
            Shelf = shelf;
            Slot = slot;
            ItemName = itemName;
        }

        public string ToLogLine() => $"{Shelf}|{Slot}|{ItemName}";

        public string ToScreenLine() => $"Изъятие | полка {Shelf} | слот {Slot} | товар «{ItemName}»";

        public static TakenEvent FromLogLine(string line)
        {
            var parts = line.Split('|');
            return new TakenEvent(
                parts[0][0],
                int.Parse(parts[1]),
                parts[2]
            );
        }
    }
}
