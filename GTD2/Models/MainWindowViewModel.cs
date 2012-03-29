﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Collections.Specialized;

namespace MyToDoList.Models
{
    [XmlRoot(ElementName = "GTD")]
    public class MainWindowViewModel
    {
        const string FileName = "data.xml";

        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        /*public byte[] FileContent
        {
            get 
            {
                using (var writer = new System.IO.MemoryStream())
                {
                    var ser = new System.Xml.Serialization.XmlSerializer(typeof(GtdViewModel));
                    ser.Serialize(writer, this);
                    writer.Position = 0;
                    return writer.ToArray();
                }
            }
            set 
            {
                using (var reader = new System.IO.MemoryStream(value))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(GtdViewModel));
                    var model = (GtdViewModel)ser.Deserialize(reader);
                    Notes = model.Notes;
                }
            }
        }*/


        private ObservableCollection<TextNoteModel> _notes;
        public ObservableCollection<TextNoteModel> Notes
        {
            get { return _notes; }
            set 
            { 
                _notes = value;
                foreach (TextNoteModel item in _notes)
                {
                    item.PropertyChanged += OnNotesChanged;
                }
                _notes.CollectionChanged += (s, e) =>
                {
                    OnNotesChanged(s, e);
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (TextNoteModel item in e.NewItems)
                        {
                            item.PropertyChanged += OnNotesChanged;
                        }
                    }
                };
            }
        }


        private void OnNotesChanged(object sender, EventArgs e)
        {
            if (NotesChanged != null)
            {
                NotesChanged(sender, e);
            }
        }

        public event EventHandler NotesChanged;

        #region Serialization

        /// <summary>
        /// Create GTD View Model with default params
        /// </summary>
        /// <returns></returns>
        private static MainWindowViewModel CreateDefault()
        {
            var model = new MainWindowViewModel()
            {
                Width = 1024,
                Height = 740,
                X = 128,
                Y = 142,

                Notes = new ObservableCollection<TextNoteModel>()
            };

            return model;
        }

        public static MainWindowViewModel Open()
        {
            if (!System.IO.File.Exists(FileName))
            {
                return CreateDefault();
            }

            MainWindowViewModel model;

            try
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(FileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(MainWindowViewModel));
                    model = (MainWindowViewModel)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not deserialize file. Probably file is corrupted.", ex);
            }

            return model;
        }

        public void Save()
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var ser = new System.Xml.Serialization.XmlSerializer(typeof(MainWindowViewModel));
                ser.Serialize(writer, this);
            }
        }

        #endregion

    }
}
