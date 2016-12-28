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

    public frmMain()
    {
      InitializeComponent();
      Cursor.Hide();
      Show();

      physics = new Physics();
      physics.RandomInitalization(300, Width, Height);
      this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
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

    private void frmMain_Click(object sender, EventArgs e)
    {
      physics.StartSimulation();
    }

    private void frmMain_Paint(object sender, PaintEventArgs e)
    {
      Physics.star[] stars = physics.Stars;
      Graphics g = e.Graphics;
      g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

      foreach (var item in stars)
      {
        g.FillEllipse(Brushes.White, (float)item.location.x, (float)item.location.y, 4, 4);
      }
    }

    private void timUpdate_Tick(object sender, EventArgs e)
    {
      this.Invalidate();
    }
  }
}