using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MyToDoList.Models
{
    [XmlType(TypeName = "TextNote")]
    public class TextNoteModel : INotifyPropertyChanged
    {
        private double _x;
        [XmlAttribute]
        public double X
        {
            get { return _x; }
            set { _x = value; OnChanged("X"); }
        }


        [XmlAttribute]
        public double Y { get; set; }

        private string _text;
        [XmlText]
        public string Text
        {
            get { return _text; }
            set { _text = value; OnChanged("Text"); }
        }


        [XmlIgnore]
        public bool EditMode { get; set; }

        protected void OnChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(null, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
