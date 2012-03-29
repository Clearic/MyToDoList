using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace MyToDoList.Controls
{
    class DragZCanvas : Canvas
    {
        bool isDraging = false;
        UIElement dragingObj;
        Point offset;

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            Point p = e.GetPosition(this);
            HitTestResult result = VisualTreeHelper.HitTest(this, p);
            isDraging = false;


            DependencyObject depObj = result.VisualHit;

            if (result.VisualHit != this)
            {
                dragingObj = result.VisualHit as UIElement;

                while (depObj != null)
                {

                    UIElement elem = depObj as UIElement;
                    if (elem != null && this.Children.Contains(elem))
                        break;

                    if (depObj is Visual)
                        depObj = VisualTreeHelper.GetParent(depObj);
                    else
                        depObj = LogicalTreeHelper.GetParent(depObj);
                }
                dragingObj = depObj as UIElement;
                isDraging = true;

                offset = new Point(Canvas.GetLeft(dragingObj) - p.X, Canvas.GetTop(dragingObj) - p.Y);

                this.CaptureMouse();

                BringToFront(dragingObj);
            }
        }

        private void BringToFront(UIElement element)
        {
            int index = Panel.GetZIndex(element);
            foreach (var ch in this.Children)
            {
                if (ch != null)
                {
                    int z = Panel.GetZIndex(ch as UIElement);
                    if (z > index) Panel.SetZIndex(ch as UIElement, z - 1);
                }
            }
            Panel.SetZIndex(element, this.Children.Count - 1);
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            isDraging = false;

            this.ReleaseMouseCapture();
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDraging)
            {
                Point p = e.GetPosition(this);
                p.Offset(offset.X, offset.Y);

                Canvas.SetLeft(dragingObj, p.X);
                Canvas.SetTop(dragingObj, p.Y);
            }
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            if (visualAdded != null)
            {
                Panel.SetZIndex(visualAdded as UIElement, this.Children.Count - 1);
            }
            else
            {
                int index = Panel.GetZIndex(visualRemoved as UIElement);
                foreach (var ch in this.Children)
                {
                    if (ch != null)
                    {
                        int i = Panel.GetZIndex(ch as UIElement);
                        if (i > index) Panel.SetZIndex(ch as UIElement, i - 1);
                    }
                }
            }

        }

    }
}
