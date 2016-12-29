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

        public Physics(int count, int width, int height, int depth)
        {
            Random rnd = new Random();
            Stars = new star[count];

            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].location.x = (rnd.NextDouble() - 0.5) * width;
                Stars[i].location.y = (rnd.NextDouble() - 0.5) * height;
                Stars[i].location.z = rnd.NextDouble() * depth + 200;

                Stars[i].speed.z = 0.00002 * Stars[i].location.x;
                Stars[i].speed.x = 0.00002 * (Stars[i].location.z - depth / 2 - 100);
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
                for (int i = 0; i < Stars.Length; i++)
                {
                    for (int j = i + 1; j < Stars.Length; j++)
                    {
                        double dis_x = (Stars[i].location.x - Stars[j].location.x);
                        double dis_y = (Stars[i].location.y - Stars[j].location.y);
                        double dis_z = (Stars[i].location.z - Stars[j].location.z);
                        double dis2 = dis_x * dis_x + dis_y * dis_y + dis_z * dis_z;

                        if (dis2 > 12)
                        {
                            double dis = Math.Sqrt(dis2);
                            double dis3 = dis2 * dis * 200.0;
                            vector speed = new vector() { x = dis_x / dis3, y = dis_y / dis3, z = dis_z / dis3 };
                            Stars[i].speed.x -= speed.x;
                            Stars[i].speed.y -= speed.y;
                            Stars[i].speed.z -= speed.z;
                            Stars[j].speed.x += speed.x;
                            Stars[j].speed.y += speed.y;
                            Stars[j].speed.z += speed.z;
                        }
                        else if (dis2 > 8 && dis2 < 12)
                        {
                            vector speed_i;
                            vector speed_j;
                            speed_i.x = (Stars[i].speed.x * 19 + Stars[j].speed.x) * 0.05;
                            speed_i.y = (Stars[i].speed.y * 19 + Stars[j].speed.y) * 0.05;
                            speed_i.z = (Stars[i].speed.z * 19 + Stars[j].speed.z) * 0.05;
                            speed_j.x = (Stars[j].speed.x * 19 + Stars[i].speed.x) * 0.05;
                            speed_j.y = (Stars[j].speed.y * 19 + Stars[i].speed.y) * 0.05;
                            speed_j.z = (Stars[j].speed.z * 19 + Stars[i].speed.z) * 0.05;
                            Stars[i].speed = speed_i;
                            Stars[j].speed = speed_j;
                        }
                        else if (dis2 < 8)
                        {
                            double speed_x = dis_x * 0.01;
                            double speed_y = dis_y * 0.01;
                            double speed_z = dis_z * 0.01;
                            Stars[i].speed.x += speed_x;
                            Stars[i].speed.y += speed_y;
                            Stars[i].speed.z += speed_z;
                            Stars[j].speed.x -= speed_x;
                            Stars[j].speed.y -= speed_y;
                            Stars[j].speed.z -= speed_z;
                        }
                    }
                }
                for (int i = 0; i < Stars.Length; i++)
                {
                    Stars[i].location.x += Stars[i].speed.x;
                    Stars[i].location.y += Stars[i].speed.y;
                    Stars[i].location.z += Stars[i].speed.z;
                }
            }
        }
    }
}
