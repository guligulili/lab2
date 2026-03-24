using System;
using System.Drawing;

namespace Lab2Graphics
{
    abstract class GraphObject
    {
        protected int x;
        protected int y;
        protected Color color;
        protected SolidBrush brush;
        protected double scale = 1.0;
        protected int originalWidth;
        protected int originalHeight;

        private static Random r = new Random();
        private static Color[] cols = { Color.Red, Color.Green, Color.Yellow, Color.Tomato, Color.Cyan };
        public static Size MaxSize { get; set; }

        public GraphObject()
        {
            color = cols[r.Next(cols.Length)];
            brush = new SolidBrush(color);
            x = r.Next(200);
            y = r.Next(200);
        }

        public double Scale
        {
            get { return scale; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Масштаб должен быть положительным!");
                scale = value;
            }
        }

        public int X
        {
            get { return x; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("X не может быть отрицательным!");
                if (value + GetCurrentWidth() > MaxSize.Width)
                    throw new ArgumentException("Выход за правую границу!");
                x = value;
            }
        }

        public int Y
        {
            get { return y; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Y не может быть отрицательным!");
                if (value + GetCurrentHeight() > MaxSize.Height)
                    throw new ArgumentException("Выход за нижнюю границу!");
                y = value;
            }
        }

        public bool Selected { get; set; }

        public int GetCurrentWidth()
        {
            return (int)(GetOriginalWidth() * scale);
        }

        public int GetCurrentHeight()
        {
            return (int)(GetOriginalHeight() * scale);
        }

        protected abstract int GetOriginalWidth();
        protected abstract int GetOriginalHeight();

        public virtual void ChangeScale(double newScale)
        {
            if (newScale <= 0)
                throw new ArgumentException("Масштаб должен быть положительным!");

            int newWidth = (int)(GetOriginalWidth() * newScale);
            int newHeight = (int)(GetOriginalHeight() * newScale);

            if (x + newWidth > MaxSize.Width || y + newHeight > MaxSize.Height)
            {
                throw new ArgumentException("Новый размер выходит за границы панели!");
            }

            scale = newScale;
        }

        public abstract void Draw(Graphics g);
        public abstract bool ContainsPoint(Point p);
    }
}