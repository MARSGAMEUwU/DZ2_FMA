using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Dz2_FMA
{
    public class FailedAttemptEvent : IJournalEntry
    {
        public string OperationType { get; }
        public char? Shelf { get; }
        public int? Slot { get; }
        public string Reason { get; }

        public FailedAttemptEvent(string operationType, char? shelf, int? slot, string reason)
        {
            OperationType = operationType;
            Shelf = shelf;
            Slot = slot;
            Reason = reason;
        }

        public string ToLogLine() => $"{OperationType}|{Shelf?.ToString() ?? "-"}|{Slot?.ToString() ?? "-"}|{Reason}";

        public string ToScreenLine()
        {
            string loc = Shelf.HasValue && Slot.HasValue ? $"полка {Shelf} слот {Slot}" :
                         Shelf.HasValue ? $"полка {Shelf}" : "-";
            return $"Неудача | {OperationType} | {loc} | причина: {Reason}";
        }

        public static FailedAttemptEvent FromLogLine(string line)
        {
            var parts = line.Split('|');
            char? shelf = parts[1] == "-" ? null : (char?)parts[1][0];
            int? slot = parts[2] == "-" ? null : (int?)int.Parse(parts[2]);
            return new FailedAttemptEvent(parts[0], shelf, slot, parts[3]);
        }
    }
}
