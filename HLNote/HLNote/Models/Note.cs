using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLiteNetExtensions.Attributes;

namespace HLNote
{
    public class Note
    {
        [PrimaryKey,AutoIncrement]
        public int NoteId { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }

        [ForeignKey(typeof(NoteContent))]
        public int ContentId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public NoteContent Content { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public Genre NoteGenre { get; set; }
        [ForeignKey(typeof(Genre))]
        public int GenreId { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public Color NoteColor { get; set; }
        [ForeignKey(typeof(Color))]
        public int ColorId { get; set; }

        [ForeignKey(typeof(Password))]
        public int PasswordId { get; set; }
        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Password NotePassword { get; set; }

        public string LastModify { get; set; }
        public bool HasPassword { get { return this.NotePassword != null; } }
        // public Password NotePassword { get; set; }
        //[MaxLength(255)]
        //public string Content { get; set; }
    }
}
