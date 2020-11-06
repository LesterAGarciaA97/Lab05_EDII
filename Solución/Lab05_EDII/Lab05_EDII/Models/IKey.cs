using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab05_EDII.Models
{
    interface IKey
    {
        string word { get; set; }
        int levels { get; set; }
        int rows { get; set; }
        int columns { get; set; }
    }
}
