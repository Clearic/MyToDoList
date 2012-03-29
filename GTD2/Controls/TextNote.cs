using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace MyToDoList.Controls
{
    class TextNote : Control
    {
        static TextNote()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextNote),new FrameworkPropertyMetadata(typeof(TextNote))); 
 
        }

        public TextNote()
        {
            MouseDoubleClick += new MouseButtonEventHandler(TextNote_MouseDoubleClick);
            LostFocus += new RoutedEventHandler(TextNote_LostFocus);
        }

        void TextNote_LostFocus(object sender, RoutedEventArgs e)
        {
            EditMode = false;
        }

        void TextNote_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditMode = true;
            e.Handled = true;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextNote), new UIPropertyMetadata("TextNote"));

        
        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(TextNote), new UIPropertyMetadata(false));

        //protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        //{
        //    //base.OnMouseDoubleClick(e);

        //    EditMode = !EditMode;
        //}

        
    }
}
