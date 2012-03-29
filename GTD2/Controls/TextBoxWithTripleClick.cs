using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyToDoList.Controls
{
    class TextBoxWithTripleClick : TextBox
    {
        protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.ClickCount == 3)
            {
                this.SelectAll();
            }

        }
    }
}
