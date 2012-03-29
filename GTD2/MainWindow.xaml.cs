using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using MyToDoList.Controls;
using MyToDoList.Models;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace MyToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            model = MainWindowViewModel.Open();
            this.DataContext = model;
            model.NotesChanged += new EventHandler(model_NotesChanged);
        }

        MainWindowViewModel model;

        #region AutoSave

        DispatcherTimer saveTimer;

        const int SaveInInterval = 5000;

        void model_NotesChanged(object sender, EventArgs e)
        {
            if (e is NotifyCollectionChangedEventArgs)
            {
                NotifyCollectionChangedEventArgs args = (NotifyCollectionChangedEventArgs)e;
                if (args.Action == NotifyCollectionChangedAction.Remove)
                {
                    Save();
                    return;
                }
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    return;
                }
            }

            SaveInterval(SaveInInterval);
        }

        private void TextNote_LostFocus(object sender, RoutedEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Save immediately 
        /// </summary>
        private void Save()
        {
            if (saveTimer != null)
            {
                saveTimer.Stop();
            }

            model.Save();
            //ShowMessage("File has been saved");
        }

        /// <summary>
        /// Save in a specified time interval 
        /// </summary>
        /// <param name="interval"></param>
        private void SaveInterval(int interval)
        {
            if (saveTimer == null)
            {
                saveTimer = new DispatcherTimer();
                saveTimer.Tick += new EventHandler(saveTimer_Tick);
            }

            saveTimer.Interval = TimeSpan.FromMilliseconds(interval);
            saveTimer.Start();
        }

        void saveTimer_Tick(object sender, EventArgs e)
        {
            Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.Save();
        }

        #endregion

        private void itemsControl1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(itemsControl1);

            itemsControl1.Focus();

            if (e.ClickCount == 2)
            {
                var textNote = new TextNoteModel()
                {
                    X = p.X - 10,
                    Y = p.Y - 10,
                    Text = "New Task",
                    EditMode = true
                };

                model.Notes.Add(textNote);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement elem = sender as FrameworkElement;
            TextNoteModel note = elem.DataContext as TextNoteModel;
            model.Notes.Remove(note);
        }

        private void ShowMessage(string text)
        {
            messageText.Text = text;
            message.Visibility = System.Windows.Visibility.Visible;
            DoubleAnimation anim = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1)));
            anim.BeginTime = TimeSpan.FromSeconds(3);
            anim.FillBehavior = FillBehavior.Stop;
            anim.Completed += (sndr, evnt) => message.Visibility = System.Windows.Visibility.Collapsed;
            message.BeginAnimation(TextBlock.OpacityProperty, anim);
        }

    }
}
