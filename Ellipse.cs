using System;
using System.Drawing;

namespace Lab2Graphics
{
    class Ellipse : GraphObject
    {
        private int baseA = 50;
        private int baseB = 50;

        public Ellipse() : base()
        {
            originalWidth = baseA * 2;
            originalHeight = baseB * 2;
        }

        public override void Draw(Graphics g)
        {
            Pen pen = Selected ? Pens.Blue : Pens.Black;
            int currentWidth = GetCurrentWidth();
            int currentHeight = GetCurrentHeight();
            g.FillEllipse(brush, x, y, currentWidth, currentHeight);
            g.DrawEllipse(pen, x, y, currentWidth, currentHeight);
        }

        public override bool ContainsPoint(Point p)
        {
            int centerX = x + GetCurrentWidth() / 2;
            int centerY = y + GetCurrentHeight() / 2;
            double currentA = GetCurrentWidth() / 2.0;
            double currentB = GetCurrentHeight() / 2.0;
            double dx = (p.X - centerX) / currentA;
            double dy = (p.Y - centerY) / currentB;
            return (dx * dx + dy * dy) <= 1;
        }

        protected override int GetOriginalWidth()
        {
            return baseA * 2;
        }

        protected override int GetOriginalHeight()
        {
            return baseB * 2;
        }
    }
}