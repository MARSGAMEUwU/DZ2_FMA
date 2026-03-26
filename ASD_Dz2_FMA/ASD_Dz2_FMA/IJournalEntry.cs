using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Dz2_FMA
{
    public interface IJournalEntry
    {
        string ToLogLine();
        string ToScreenLine();
    }
}
