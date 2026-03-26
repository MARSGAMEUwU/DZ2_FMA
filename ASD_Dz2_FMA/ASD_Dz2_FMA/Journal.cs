using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Dz2_FMA
{
    public class Journal<T> where T : IJournalEntry
    {
        private readonly List<T> entries = new List<T>();

        public void Add(T entry) => entries.Add(entry);

        public IEnumerable<T> GetAll() => entries.AsReadOnly();

        public int Count => entries.Count;
    }
}
