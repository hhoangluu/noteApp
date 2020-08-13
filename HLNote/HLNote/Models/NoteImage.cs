using System;
using System.Collections.Generic;
using System.Text;
using SQLiteNetExtensions.Attributes;
using SQLite;

namespace HLNote
{
    public class NoteImage
    {
        [PrimaryKey, AutoIncrement]
        public int ImageId { get; set; }
        [ForeignKey(typeof(NoteContent))]
        public int NoteId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
