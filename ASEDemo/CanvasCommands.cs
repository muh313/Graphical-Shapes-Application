using System.Drawing;

namespace ASEDemo
{
    public class CanvasCommands
    {
        // instance data
        readonly Graphics g;
        readonly Pen pen;
        int xPos, yPos; //pens position

        /// <summary>
        /// Creates basic tools for canvas.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="varCommand"></param>
        public CanvasCommands(Graphics g, VarCommand varCommand)
        {
            this.g = g;
            xPos = yPos = 5;
            pen = new Pen(Color.Red, 1);

            g.DrawEllipse(pen, xPos, yPos, 5, 5); //makes that dot, so pens visible
        }

        /// <summary>
        /// Fills the pen.
        /// </summary>
        /// <param name="mode"></param>
        public void FillPen(string mode)
        {
            if (mode == "on")
            {
                pen.Width = 100;
            }
            if (mode == "off")
            {
                pen.Width = 0;
            }
        }

        /// <summary>
        /// Changes the pen colour.
        /// </summary>
        /// <param name="a"></param>
        public void PenColour(string a)
        {
            if (a == "red")
            {
                pen.Color = Color.Red;
            }
            else if (a == "green")
            {
                pen.Color = Color.Green;
            }
            else if (a == "black")
            {
                pen.Color = Color.Black;
            }
            else if (a == "blue")
            {
                pen.Color = Color.Blue;
            }
        }

        /// <summary>
        /// Moves the pen.
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="loc2"></param>
        public void MovePen(int loc, int loc2)
        {
            xPos = loc;
            yPos = loc2;
            g.DrawEllipse(pen, xPos, yPos, 5, 5); //makes that dot, so pens visible
        }

        /// <summary>
        /// Clears drawings off canvas.
        /// </summary>
        public void Clear()
        {
            g.Clear(SystemColors.Control);
        }

        /// <summary>
        /// Resets canvas position.
        /// </summary>
        public void Reset()
        {
            MovePen(0, 0);    //moves pen back to normal
        }

        /// <summary>
        /// Draws line(s).
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        public void DrawTo(int toX, int toY)
        {
            g.DrawLine(pen, xPos, yPos, toX, toY);
            xPos = toX;
            yPos = toY;
        }

        /// <summary>
        /// Draws circle.
        /// </summary>
        /// <param name="radius"></param>
        public void DrawCircle(float radius)
        {
            g.DrawEllipse(pen, xPos, yPos, radius + radius, radius + radius);   //so one sole value (R) as param
        }

        /// <summary>
        /// Draws rectangle.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DrawRectangle(int width, int height)
        {
            g.DrawRectangle(pen, xPos, yPos, width, height);
        }

        /// <summary>
        /// Draws triangle.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        public void DrawTriangle(int point1, int point2)
        {
            int x = point1;
            int y = point2;

            Point[] points = {          //can draw any triangle with this
                new Point(100, 100),
                new Point(65, x),       // 30,15
                new Point(y, 100)
            };
            g.DrawPolygon(pen, points);
        }
    }
}