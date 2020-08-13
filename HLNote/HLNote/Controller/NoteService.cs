using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HLNote
{
    class NoteService
    {
        private ObservableCollection<Note> _notes;
        private DatabaseConnector<Note> _dbConnectorNote;
        private DatabaseConnector<Genre> _dbConnectorGenre;
        private DatabaseConnector<Color> _dbConnectorColor;
        private DatabaseConnector<NoteImage> _dbConnectorImage;
        private DatabaseConnector<Password> _dbConnectorPassword;
        private DatabaseConnector<Question> _dbConnectorQuestion;
        private DatabaseConnector<NoteContent> _dbConnectorContent;
        public NoteService()
        {
            DataInit();
        }
        public NoteService(Note[] notes)
        {
            _notes = new ObservableCollection<Note>(notes);
        }
        public ObservableCollection<Note> Notes { get { return _notes; } set { _notes = value; } }

        void DataInit()
        {
            //create table in sqlite database
            _dbConnectorColor = new DatabaseConnector<Color>();
            _dbConnectorGenre = new DatabaseConnector<Genre>();
            _dbConnectorNote = new DatabaseConnector<Note>();
            _dbConnectorImage = new DatabaseConnector<NoteImage>();
            _dbConnectorPassword = new DatabaseConnector<Password>();
            _dbConnectorQuestion = new DatabaseConnector<Question>();
            _dbConnectorContent = new DatabaseConnector<NoteContent>();
            //this.BackgroundColor = Xamarin.Forms.Color.Red;
        }
        public void GetData()
        {
            _notes = _dbConnectorNote.LoadData();
        }

        public IEnumerable<Note> GetNotes(string searchText = null)
        {
            if (String.IsNullOrWhiteSpace(searchText))
                return _notes.OrderByDescending(c=>DateTime.ParseExact(c.LastModify, "dd-MM-yyyy HH:mm tt", null));
            IEnumerable<Note> result;
            //if (String.IsNullOrEmpty(searchText))
            //result = _notes.Where(c => c.Title.StartsWith(searchText, StringComparison.CurrentCultureIgnoreCase));
            //else
            //{
            result = _notes.Where(c => c.Title.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) != -1 ||(
                                       c.NoteGenre!=null&&c.NoteGenre.GenreName.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) != -1))
                                       .OrderByDescending(c=> DateTime.ParseExact(c.LastModify, "dd-MM-yyyy HH:mm tt", null));
            //}
            return result;
        }
        public bool RemoveNote(Note note)
        {
            if (_notes == null)
                throw new NullReferenceException();
            _dbConnectorNote.RemoveData(note);
            return (_notes.Remove(note));
        }
        public void ReplaceNote(Note note)
        {
            if (_notes == null)
                throw new NullReferenceException();
            _dbConnectorNote.UpdateData(note);
            var preUpdate = _notes.Single(c => c.NoteId == note.NoteId);
            var index = _notes.IndexOf(preUpdate);
            _notes[index] = note;
        }
        public void AddNote(Note note)
        {
            if (_notes == null)
                throw new NullReferenceException();
            _dbConnectorNote.InsertData(note);
            _notes.Add(note);
        }
    }
}
