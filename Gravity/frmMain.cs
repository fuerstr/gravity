using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gravity
{
    public partial class frmMain : Form
    {
        DateTime lastPaint;
        int framesPerSecond;

        struct Camera
        {
            public Physics.Vector location;
        }

        Physics physics;
        Camera camera;
        Point lastMouseLocation;
        bool run;

        Brush brush1;
        Brush brush2;
        Brush brush3;
        Brush brush4;

        public frmMain()
        {
            InitializeComponent();

            brush1 = new SolidBrush(Color.FromArgb(255, Color.White));
            brush2 = new SolidBrush(Color.FromArgb(100, Color.White));
            brush3 = new SolidBrush(Color.FromArgb(50, Color.White));
            brush4 = new SolidBrush(Color.FromArgb(25, Color.White));
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Cursor.Hide();

            camera.location.z = -100;

            physics = new Physics(2000, Width, Height, 1000);
            timUpdate.Start();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.Left:
                    camera.location.x += 3;
                    break;
                case Keys.Right:
                    camera.location.x -= 3;
                    break;
                case Keys.Up:
                    if (e.Shift) camera.location.z += 3;
                    else camera.location.y += 3;
                    break;
                case Keys.Down:
                    if (e.Shift) camera.location.z -= 3;
                    else camera.location.y -= 3;
                    break;
                case Keys.Space:
                    if (run) physics.StopSimulation();
                    else physics.StartSimulation();
                    run = !run;
                    break;
            }
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            Physics.Star[] stars = physics.Stars;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            float center_x = Width / 2;
            float center_y = Height / 2;

            foreach (var item in stars)
            {
                if ((item.location.z - camera.location.z) > 10)
                {
                    float zoom = 400 / (float)(item.location.z - camera.location.z);
                    float x = (float)((item.location.x + camera.location.x) * zoom + center_x);
                    float y = (float)((item.location.y + camera.location.y) * zoom + center_y);
                    zoom *= 0.3f;
                    Draw_Point(g, brush1, x, y, 3 * zoom);
                    Draw_Point(g, brush2, x, y, 6 * zoom);
                    Draw_Point(g, brush3, x, y, 9 * zoom);
                    Draw_Point(g, brush4, x, y, 12 * zoom);
                }
            }

            double frameTime = DateTime.Now.Subtract(lastPaint).TotalSeconds;
            if (frameTime > 1)
            {
                framesPerSecond = (int)(physics.Count / frameTime);
                physics.Count = 0;
                lastPaint = DateTime.Now;
            }
            e.Graphics.DrawString(framesPerSecond.ToString(), Font, Brushes.White, new Point(10, 19));
        }

        private void Draw_Point(Graphics g, Brush brush, float x, float y, float size)
        {
            g.FillEllipse(brush, x - (size / 2.0f), y - (size / 2.0f), size, size);
        }

        private void timUpdate_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            lastMouseLocation = e.Location;
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                camera.location.x += e.Location.X - lastMouseLocation.X;
                camera.location.y += e.Location.Y - lastMouseLocation.Y;
                lastMouseLocation = e.Location;
            }
        }

    }
}