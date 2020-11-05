
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab05_EDII.Models
{
    public class Key : IKey
    {
        public string Word { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Levels { get; set; }
    }
}
