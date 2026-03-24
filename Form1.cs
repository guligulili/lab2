using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Lab2Graphics
{
    public partial class Form1 : Form
    {
        private List<GraphObject> elements = new List<GraphObject>();
        private Random random = new Random();
        private GraphObject draggedObject = null;
        private int dragOffsetX;
        private int dragOffsetY;
        private bool dragging = false;
        private GraphObject selectedObject = null;
        private double[] scaleSteps = { 1.0, 2.0, 3.0, 4.0, 5.0 };

        public Form1()
        {
            InitializeComponent();
            GraphObject.MaxSize = panel1.ClientSize;
            this.Resize += Form1_Resize;
            if (toolStripStatusLabel1 != null)
            {
                toolStripStatusLabel1.Text = "Готов к работе";
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            GraphObject.MaxSize = panel1.ClientSize;
            panel1.Invalidate();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                GraphObject newObject = CreateRandomFigure();
                int maxX = Math.Max(0, panel1.ClientSize.Width - 100);
                int maxY = Math.Max(0, panel1.ClientSize.Height - 100);
                newObject.X = random.Next(maxX);
                newObject.Y = random.Next(maxY);
                elements.Add(newObject);
                if (toolStripStatusLabel1 != null)
                {
                    string typeName = newObject is Rectangle ? "Прямоугольник" : "Эллипс";
                    toolStripStatusLabel1.Text = $"Добавлен {typeName}. Всего фигур: {elements.Count}";
                }
                panel1.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании фигуры: " + ex.Message);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            elements.Clear();
            dragging = false;
            draggedObject = null;
            selectedObject = null;
            if (toolStripStatusLabel1 != null)
            {
                toolStripStatusLabel1.Text = "Все фигуры удалены";
            }
            panel1.Invalidate();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы действительно хотите выйти?",
                "Подтверждение выхода",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            foreach (GraphObject obj in elements)
            {
                obj.Draw(e.Graphics);
            }
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    GraphObject newObject = CreateRandomFigure();
                    newObject.X = e.X;
                    newObject.Y = e.Y;
                    elements.Add(newObject);

                    if (toolStripStatusLabel1 != null)
                    {
                        string typeName = newObject is Rectangle ? "Прямоугольник" : "Эллипс";
                        toolStripStatusLabel1.Text = $"Создан {typeName} в точке ({e.X}, {e.Y})";
                    }

                    panel1.Invalidate();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Ошибка при создании фигуры: " + ex.Message);
                }
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            
            foreach (GraphObject obj in elements)
            {
                obj.Selected = false;
            }
            selectedObject = null;

            
            for (int i = elements.Count - 1; i >= 0; i--)
            {
                if (elements[i].ContainsPoint(e.Location))
                {
                    
                    elements[i].Selected = true;
                    selectedObject = elements[i];

                    
                    draggedObject = elements[i];
                    dragOffsetX = e.X - draggedObject.X;
                    dragOffsetY = e.Y - draggedObject.Y;
                    dragging = true;

                    if (toolStripStatusLabel1 != null)
                    {
                        string typeName = draggedObject is Rectangle ? "Прямоугольник" : "Эллипс";
                        toolStripStatusLabel1.Text = $"Выделен: {typeName} (масштаб: {draggedObject.Scale:F1}x)";
                    }
                    break;
                }
            }

            panel1.Invalidate();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && draggedObject != null)
            {
                try
                {
                    draggedObject.X = e.X - dragOffsetX;
                    draggedObject.Y = e.Y - dragOffsetY;
                    panel1.Invalidate();
                }
                catch (ArgumentException)
                {
                    
                }
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            draggedObject = null;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Right)
            {
                
                GraphObject clickedObject = null;
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    if (elements[i].ContainsPoint(e.Location))
                    {
                        clickedObject = elements[i];
                        break;
                    }
                }

                
                if (clickedObject != null)
                {
                    try
                    {
                        double currentScale = clickedObject.Scale;
                        int index = Array.IndexOf(scaleSteps, currentScale);
                        double newScale = (index >= 0 && index < scaleSteps.Length - 1)
                            ? scaleSteps[index + 1]
                            : scaleSteps[0];

                        clickedObject.ChangeScale(newScale);

                        if (toolStripStatusLabel1 != null)
                        {
                            string scaleText = newScale == 1.0 ? "исходный размер" :
                                              newScale == 2.0 ? "увеличен в 2 раза" :
                                              newScale == 3.0 ? "увеличен в 3 раза" :
                                              newScale == 4.0 ? "увеличен в 4 раза" :
                                              "увеличен в 5 раз";

                            string typeName = clickedObject is Rectangle ? "Прямоугольник" : "Эллипс";
                            toolStripStatusLabel1.Text = $"{typeName}: {scaleText}";
                        }

                        panel1.Invalidate();
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show($"Нельзя изменить размер: {ex.Message}");
                    }
                }
            }
        }

        private GraphObject CreateRandomFigure()
        {
            if (random.Next(2) == 0)
                return new Rectangle();
            else
                return new Ellipse();
        }

        private GraphObject GetSelectedObject()
        {
            foreach (GraphObject obj in elements)
            {
                if (obj.Selected)
                    return obj;
            }
            return null;
        }
    }
}