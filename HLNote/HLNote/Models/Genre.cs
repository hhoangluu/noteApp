using SQLite;

namespace HLNote
{
    public class Genre
    {
        [PrimaryKey,AutoIncrement]
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public Genre()
        {

        }
        public Genre(Genre genre)
        {
            GenreId = genre.GenreId;
            GenreName = genre.GenreName;
        }

    }
}
