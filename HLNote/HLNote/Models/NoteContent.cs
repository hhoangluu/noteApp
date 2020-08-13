using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLNote
{
    public class NoteContent
    {
        [PrimaryKey, AutoIncrement]
        public int ContentId { get; set; }
        public string Content { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<NoteImage> Images { get; set; }
        public bool RemoveImage(NoteImage image)
        {
            return Images.Remove(image);
        }
        public void AddImage(NoteImage image)
        {
            Images.Add(image);
        }
    }
}
