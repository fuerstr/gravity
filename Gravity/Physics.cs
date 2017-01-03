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
        public struct Vector
        {
            public double x;
            public double y;
            public double z;
        }

        public struct Star
        {
            public Vector location;
            public Vector speed;
        }

        public Star[] Stars { private set; get; }
        Thread thrSimulation;

        public int Count { set; get; }

        public Physics(int count, int width, int height, int depth)
        {
            Random rnd = new Random();
            Stars = new Star[count];

            for (int i = 0; i < Stars.Length; i++)
            {
                double angle = rnd.NextDouble() * Math.PI * 2;
                double radius = rnd.NextDouble() * Math.Pow(width * 0.5, 2);
                Stars[i].location.x = Math.Cos(angle) * Math.Sqrt(radius);
                Stars[i].location.y = Math.Sin(angle) * Math.Sqrt(radius);
                Stars[i].location.z = rnd.NextDouble() * depth + 200;

                if (Stars[i].location.x > 0)
                    Stars[i].speed.y = 0.025 * Math.Sqrt(Stars[i].location.x);
                else
                    Stars[i].speed.y = -0.025 * Math.Sqrt(-Stars[i].location.x);

                if (Stars[i].location.y > 0)
                    Stars[i].speed.x = -0.025 * Math.Sqrt(Stars[i].location.y);
                else
                    Stars[i].speed.x = 0.025 * Math.Sqrt(-Stars[i].location.y);

            }
        }

        public void StartSimulation()
        {
            thrSimulation = new Thread(RunSimulation);
            thrSimulation.IsBackground = true;
            thrSimulation.Start();
        }

        public void StopSimulation()
        {
            if (thrSimulation != null)
            {
                thrSimulation.Abort();
                thrSimulation = null;
            }
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
            Count++;
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

                        if (dis2 > 30)
                        {
                            double dis = Math.Sqrt(dis2);
                            double dis3 = dis2 * dis * 1.0;
                            Vector speed = new Vector() { x = dis_x / dis3, y = dis_y / dis3, z = dis_z / dis3 };
                            Stars[i].speed.x -= speed.x;
                            Stars[i].speed.y -= speed.y;
                            Stars[i].speed.z -= speed.z;
                            Stars[j].speed.x += speed.x;
                            Stars[j].speed.y += speed.y;
                            Stars[j].speed.z += speed.z;
                        }
                        if (dis2 < 2)
                        {
                            Vector speed_i;
                            Vector speed_j;
                            speed_i.x = (Stars[i].speed.x * 19 + Stars[j].speed.x) * 0.05;
                            speed_i.y = (Stars[i].speed.y * 19 + Stars[j].speed.y) * 0.05;
                            speed_i.z = (Stars[i].speed.z * 19 + Stars[j].speed.z) * 0.05;
                            speed_j.x = (Stars[j].speed.x * 19 + Stars[i].speed.x) * 0.05;
                            speed_j.y = (Stars[j].speed.y * 19 + Stars[i].speed.y) * 0.05;
                            speed_j.z = (Stars[j].speed.z * 19 + Stars[i].speed.z) * 0.05;
                            Stars[i].speed = speed_i;
                            Stars[j].speed = speed_j;
                        }
                        if (dis2 < 10)
                        {
                            double dis3 = 100;
                            Vector speed = new Vector() { x = dis_x / dis3, y = dis_y / dis3, z = dis_z / dis3 };
                            Stars[i].speed.x += speed.x;
                            Stars[i].speed.y += speed.y;
                            Stars[i].speed.z += speed.z;
                            Stars[j].speed.x -= speed.x;
                            Stars[j].speed.y -= speed.y;
                            Stars[j].speed.z -= speed.z;
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
