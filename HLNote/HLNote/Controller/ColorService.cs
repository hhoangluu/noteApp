using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HLNote
{
    class ColorService
    {
        private ObservableCollection<Color> _colors;
        private DatabaseConnector<Color> _dbConnector;
        public ColorService()
        {
            _dbConnector = new DatabaseConnector<Color>();
        }
        public ColorService(Color[] colors)
        {
            _colors = new ObservableCollection<Color>(colors);
        }
        public void GetData()
        {
            if (!_dbConnector.HasData())
            {
                var ColorList = new List<Color> {
                    new Color { ColorName = "#ffffff"},
                    new Color { ColorName= "#f28b82"},   //Red
                    new Color { ColorName = "#ccff90"},  //Green
                    new Color { ColorName = "#cbf0f8"},  //Blue
                    new Color { ColorName = "#fff475"},  //Yellow
                };
                foreach (var Color in ColorList)
                    _dbConnector.InsertData(Color);
            }
            _colors = _dbConnector.LoadData();
        }
        public ObservableCollection<Color> Colors { get { return _colors; } set { _colors = value; } }
        public bool RemoveColor(Color color)
        {
            if (_colors == null) return false;
            return _colors.Remove(color);
        }
        public void AddColor(Color color)
        {
            if (_colors == null)
                return;
            _colors.Add(color);
        }
    }
}
