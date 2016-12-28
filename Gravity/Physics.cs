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

        public star[] Stars { private set; get; }
        Thread thrSimulation;

        public Physics(int count, int width, int height)
        {
            Random rnd = new Random();
            Stars = new star[count];

            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].location.x = rnd.NextDouble() * width;
                Stars[i].location.y = rnd.NextDouble() * height;
                Stars[i].location.z = rnd.NextDouble();

                Stars[i].speed.y = 0.00001 * (Stars[i].location.x - (width / 2));
                Stars[i].speed.x = -0.00001 * (Stars[i].location.y - (height / 2));
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
            unsafe
            {
                {
                    for (int i = 0; i < Stars.Length; i++)
                    {
                        for (int j = i + 1; j < Stars.Length; j++)
                        {
                            double dis_x = (Stars[i].location.x - Stars[j].location.x);
                            double dis_y = (Stars[i].location.y - Stars[j].location.y);
                            double dis2 = dis_x * dis_x + dis_y * dis_y;

                            if (dis2 > 5)
                            {
                                double dis = Math.Sqrt(dis2);
                                double dis3 = dis2 * dis * 1000.0;
                                double speed_x = dis_x / dis3;
                                double speed_y = dis_y / dis3;
                                Stars[i].speed.x -= speed_x;
                                Stars[i].speed.y -= speed_y;
                                Stars[j].speed.x += speed_x;
                                Stars[j].speed.y += speed_y;
                            }
                            else if (dis2 < 1)
                            {
                                Stars[i].speed.x = (Stars[i].speed.x + Stars[j].speed.x) * 0.5;
                                Stars[i].speed.y = (Stars[i].speed.y + Stars[j].speed.y) * 0.5;
                                Stars[j].speed.x = Stars[i].speed.x;
                                Stars[j].speed.y = Stars[i].speed.y;
                            }
                        }
                    }
                    for (int i = 0; i < Stars.Length; i++)
                    {
                        Stars[i].location.x += Stars[i].speed.x;
                        Stars[i].location.y += Stars[i].speed.y;
                    }
                }
            }
        }
    }
}
