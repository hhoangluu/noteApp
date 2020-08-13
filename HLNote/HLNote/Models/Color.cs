using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLNote
{
    public class Color
    {
        [PrimaryKey,AutoIncrement]
        public int ColorId { get; set; }
        public string ColorName { get; set; }
    }
}
