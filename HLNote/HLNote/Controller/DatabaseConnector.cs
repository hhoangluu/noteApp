using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLiteNetExtensions.Extensions;

namespace HLNote
{
    class DatabaseConnector<T> where T : new()
    {
        private SQLiteConnection _connection;
        public DatabaseConnector()
        {
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            _connection.CreateTable<T>();
        }
        public ObservableCollection<T> LoadData()
        {
            var notes = _connection.GetAllWithChildren<T>(recursive: true);
            return new ObservableCollection<T>(notes);
        }
        public void RemoveData(T note)
        {
            _connection.Delete(note);
        }
        public void InsertData(T note)
        {
            //_connection.Insert(note);
            _connection.InsertOrReplaceWithChildren(note,recursive:true);
        }
        public void UpdateData(T note)
        {
            _connection.InsertOrReplaceWithChildren(note,recursive:true);
        }
        public bool HasData()
        {
             var dataset = _connection.GetAllWithChildren<T>();
            return dataset.Count > 0;
        }
    }

}
