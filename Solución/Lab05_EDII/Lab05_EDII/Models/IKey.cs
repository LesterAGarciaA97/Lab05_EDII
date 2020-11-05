using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab05_EDII.Models
{
    interface IKey
    {
        string Word { get; set; }
        int Levels { get; set; }
        int Rows { get; set; }
        int Columns { get; set; }
    }
}
