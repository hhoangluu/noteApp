using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HLNote
{
    public interface ISQLiteDb
    {
        SQLiteConnection GetConnection();
    }
}
