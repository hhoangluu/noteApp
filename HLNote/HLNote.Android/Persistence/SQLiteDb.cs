using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HLNote;
using HLNote.Droid;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace HLNote.Droid
{
    class SQLiteDb : ISQLiteDb
    {
        public SQLiteConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(documentsPath, "HLNoteData10.db3");
            return new SQLiteConnection(path);
        }
    }
}