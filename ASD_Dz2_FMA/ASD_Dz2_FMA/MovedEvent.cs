using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Dz2_FMA
{
    public class MovedEvent : IJournalEntry
    {
        public char SourceShelf { get; }
        public int SourceSlot { get; }
        public char DestShelf { get; }
        public int DestSlot { get; }
        public string ItemName { get; }

        public MovedEvent(char sourceShelf, int sourceSlot, char destShelf, int destSlot, string itemName)
        {
            SourceShelf = sourceShelf;
            SourceSlot = sourceSlot;
            DestShelf = destShelf;
            DestSlot = destSlot;
            ItemName = itemName;
        }

        public string ToLogLine() => $"{SourceShelf}|{SourceSlot}|{DestShelf}|{DestSlot}|{ItemName}";

        public string ToScreenLine() => $"Перенос | с {SourceShelf}:{SourceSlot} на {DestShelf}:{DestSlot} | товар «{ItemName}»";

        public static MovedEvent FromLogLine(string line)
        {
            var parts = line.Split('|');
            return new MovedEvent(
                parts[0][0],
                int.Parse(parts[1]),
                parts[2][0],
                int.Parse(parts[3]),
                parts[4]
            );
        }
    }
}
