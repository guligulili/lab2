using System;
using System.Drawing;

namespace Lab2Graphics
{
    class Rectangle : GraphObject
    {
        private int baseWidth = 50;
        private int baseHeight = 50;

        public Rectangle() : base()
        {
            originalWidth = baseWidth;
            originalHeight = baseHeight;
        }

        public override void Draw(Graphics g)
        {
            Pen pen = Selected ? Pens.Blue : Pens.Black;
            int currentWidth = GetCurrentWidth();
            int currentHeight = GetCurrentHeight();
            g.FillRectangle(brush, x, y, currentWidth, currentHeight);
            g.DrawRectangle(pen, x, y, currentWidth, currentHeight);
        }

        public override bool ContainsPoint(Point p)
        {
            int currentWidth = GetCurrentWidth();
            int currentHeight = GetCurrentHeight();
            return p.X >= x && p.X <= x + currentWidth &&
                   p.Y >= y && p.Y <= y + currentHeight;
        }

        protected override int GetOriginalWidth()
        {
            return baseWidth;
        }

        protected override int GetOriginalHeight()
        {
            return baseHeight;
        }
    }
}