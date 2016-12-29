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
        Physics physics;

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

            physics = new Physics(300, Width, Height, 1000);
            physics.StartSimulation();
            timUpdate.Start();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            Physics.star[] stars = physics.Stars;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            float center_x = Width / 2;
            float center_y = Height / 2;

            foreach (var item in stars)
            {
                if (item.location.z > 20)
                {
                    float zoom = 400 / (float)item.location.z;
                    float x = center_x + (float)(item.location.x * zoom);
                    float y = center_y + (float)(item.location.y * zoom);

                    Draw_Point(g, brush1, x, y, 3 * zoom);
                    Draw_Point(g, brush2, x, y, 6 * zoom);
                    Draw_Point(g, brush3, x, y, 9 * zoom);
                    Draw_Point(g, brush4, x, y, 12 * zoom);
                }
            }
        }

        private void Draw_Point(Graphics g, Brush brush, float x, float y, float size)
        {
            g.FillEllipse(brush, x - (size / 2.0f), y - (size / 2.0f), size, size);
        }

        private void timUpdate_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}