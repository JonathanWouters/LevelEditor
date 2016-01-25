using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DirectxWpf.Behavior
{
    class FrameworkElementAdorner : Adorner
    {
        private AdornerLayer adornerLayer;

        public FrameworkElementAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            this.adornerLayer = AdornerLayer.GetAdornerLayer(this.AdornedElement);
            this.adornerLayer.Add(this);
        }

        internal void Update()
        {
            this.adornerLayer.Update(this.AdornedElement);
            this.Visibility = System.Windows.Visibility.Visible;
        }

        public void Remove()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            //SolidColorBrush renderBrush = new SolidColorBrush(Colors.DarkCyan);
            //renderBrush.Opacity = 0.5;
            Pen renderPen = new Pen(new SolidColorBrush(Colors.White), 2.5);

            Point leftPoint =new Point(adornedElementRect.BottomLeft.X - 2, adornedElementRect.BottomLeft.Y);
            Point RightPoint = new Point(adornedElementRect.BottomRight.X + 2, adornedElementRect.BottomRight.Y);

            // Draw a Line under the object.
            drawingContext.DrawLine(renderPen, leftPoint,RightPoint);

        }

    }
}
