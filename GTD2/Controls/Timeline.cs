using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace MyToDoList.Controls
{
    public class Timeline : FrameworkElement
    {
        public Timeline()
            : base()
        {
            System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
            dTimer.Interval = TimeSpan.FromMinutes(1);
            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Start();
        }

        void dTimer_Tick(object sender, EventArgs e)
        {
            this.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Brushes.Gray, 1);
            Pen penDash = new Pen(Brushes.Silver, 1) { DashStyle = DashStyles.Dash };
            Rect rect = new Rect(20, 20, 50, 60);
            
            double halfPenWidth = pen.Thickness / 2;

            TimeSpan StartTime = TimeSpan.FromHours(6);
            TimeSpan EndTime = TimeSpan.FromHours(24);

            GuidelineSet guidelines = new GuidelineSet();
            guidelines.GuidelinesX.Add(rect.Left + halfPenWidth);
            guidelines.GuidelinesX.Add(rect.Right + halfPenWidth);
            guidelines.GuidelinesY.Add(rect.Top + halfPenWidth);
            guidelines.GuidelinesY.Add(rect.Bottom + halfPenWidth);

            drawingContext.PushGuidelineSet(guidelines);
            
            TimeSpan tmpTime = StartTime;
            int drawHours = (int)EndTime.Subtract(StartTime).TotalHours;
            double segmentHeight = this.ActualHeight / drawHours;
            for (int i = 0; i < drawHours; i++)
            {
                int y = (int)(segmentHeight * i);
                drawingContext.DrawLine(pen, new Point(0, y), new Point(this.ActualWidth, y));
                int y2 = (int)(y + segmentHeight / 2);
                drawingContext.DrawLine(penDash, new Point(0, y2), new Point(this.ActualWidth, y2));
                drawingContext.DrawText(new FormattedText(
                    tmpTime.ToString(@"h\:mm"),
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.RightToLeft, 
                    new Typeface("Segoe UI"),
                    12,
                    Brushes.Black, 
                    null, 
                    TextFormattingMode.Display),new Point(this.ActualWidth, y));
                tmpTime = tmpTime.Add(TimeSpan.FromHours(1));
            }
            
            TimeSpan timeNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).Subtract(StartTime);
            
            double h = segmentHeight * (timeNow.TotalHours);
            drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(0, h), new Point(this.ActualWidth, h));
            
            drawingContext.Pop();
        }
    }
}
