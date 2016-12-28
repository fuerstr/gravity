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

            physics = new Physics(600, Width, Height);
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

            foreach (var item in stars)
            {
                g.FillEllipse(brush1, (float)item.location.x - 1, (float)item.location.y - 1, 2, 2);
                g.FillEllipse(brush2, (float)item.location.x - 2, (float)item.location.y - 2, 4, 4);
                g.FillEllipse(brush3, (float)item.location.x - 4, (float)item.location.y - 4, 8, 8);
                g.FillEllipse(brush3, (float)item.location.x - 6, (float)item.location.y - 6, 12, 12);
            }
        }

        private void timUpdate_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}