using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HLNote
{
    class GenreService
    {
        private ObservableCollection<Genre> _genres;
        private DatabaseConnector<Genre> _dbConnector;
        public GenreService()
        {
            _dbConnector = new DatabaseConnector<Genre>();
        }
        public void GetData()
        {
            if(!_dbConnector.HasData())
            {
                var genreList = new List<Genre> {
                    new Genre { GenreName = "Love"},
                    new Genre { GenreName = "Study"},
                    new Genre { GenreName = "Entertainment"},
                    new Genre { GenreName = "Shopping"},
                    new Genre { GenreName = "Progamming"},
                };
                foreach (var genre in genreList)
                    _dbConnector.InsertData(genre);
            }
            _genres = _dbConnector.LoadData();
        }
        public GenreService(Genre[] genres)
        {
            _genres = new ObservableCollection<Genre>(genres);
        }
        public ObservableCollection<Genre> Genres { get { return _genres; } set { _genres = value; } }
        public bool RemoveGenre(Genre genre)
        {
            if (_genres == null)
                return false;
            _dbConnector.RemoveData(genre);
            return _genres.Remove(genre);
        }
        public void ReplaceGenre(Genre genre)
        {
            if (_genres == null)
                return;
            _dbConnector.UpdateData(genre);
            var preUpdate = _genres.Single(c=>c.GenreId == genre.GenreId);
            var index = _genres.IndexOf(preUpdate);
            _genres[index] = genre;
        }
        public void AddGenre(Genre genre)
        {
            if (_genres == null)
                return;
            _dbConnector.InsertData(genre);
            _genres.Add(genre);
        }
    }
}
