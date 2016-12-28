using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gravity
{
  public class Physics
  {
    public struct vector
    {
      public double x;
      public double y;
      public double z;
    }

    public struct star
    {
      public vector location;
      public vector speed;
    }

    star[] stars = new star[300];
    Thread thrSimulation;

    public star[] Stars
    {
      get
      {
        lock (stars)
        {
          return stars;
        }
      }
    }

    public void RandomInitalization(int count, int width, int height)
    {
      Random rnd = new Random();

      lock (stars)
      {

        stars = new star[count];

        for (int i = 0; i < stars.Length; i++)
        {
          stars[i].location.x = rnd.NextDouble() * width;
          stars[i].location.y = rnd.NextDouble() * height;
          stars[i].location.z = rnd.NextDouble();
        }
      }
    }

    public void StartSimulation()
    {
      thrSimulation = new Thread(RunSimulation);
      thrSimulation.IsBackground = true;
      thrSimulation.Start();
    }

    private void RunSimulation()
    {
      while (true)
      {
        Calculate_OneStep();
      }
    }

    private void Calculate_OneStep()
    {
      lock (stars)
      {
        unsafe
        {
          {
            for (int i = 0; i < stars.Length; i++)
            {
              for (int j = i + 1; j < stars.Length; j++)
              {
                double dis_x = (stars[i].location.x - stars[j].location.x);
                double dis_y = (stars[i].location.y - stars[j].location.y);
                double dis2 = dis_x * dis_x + dis_y * dis_y;

                if (dis2 > 8)
                {
                  double dis = Math.Sqrt(dis2);
                  double dis3 = dis2 * dis * 1000.0;
                  double speed_x = dis_x / dis3;
                  double speed_y = dis_y / dis3;
                  stars[i].speed.x -= speed_x;
                  stars[i].speed.y -= speed_y;
                  stars[j].speed.x += speed_x;
                  stars[j].speed.y += speed_y;
                }
                else
                {
                  stars[i].speed.x = (stars[i].speed.x + stars[j].speed.x) / 2;
                  stars[i].speed.y = (stars[i].speed.y + stars[j].speed.y) / 2;
                  stars[j].speed.x = stars[i].speed.x;
                  stars[j].speed.y = stars[i].speed.y;
                }
              }
            }
            for (int i = 0; i < stars.Length; i++)
            {
              stars[i].location.x += stars[i].speed.x;
              stars[i].location.y += stars[i].speed.y;
            }
          }
        }
      }
    }
  }
}
