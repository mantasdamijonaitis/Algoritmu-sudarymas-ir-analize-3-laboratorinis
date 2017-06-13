using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsThird
{
    public class SecondTask
    {

        private const int n = 450000000;
        private const int W = 2;
        private static int[] w = new int[n];
        private static int[] v = new int[n];
        private class CustomData
        {
            public int TNum;
            public int TResult;
        }

        public SecondTask()
        {
            for (int i = 0; i < n; i++)
            {
                w[i] = i + 5;
                v[i] = i + 5;
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int simpleResult = F(W, w, v);
            stopWatch.Stop();
            Console.WriteLine("simpleResult dataAmount={0}, time={1}",
                w.Length, stopWatch.ElapsedTicks);
            stopWatch.Reset();
            stopWatch.Start();
            int parallelResult = ParallelF(W);
            stopWatch.Stop();
            Console.WriteLine("parallel dataAmount={0}, time={1}",
                w.Length, stopWatch.ElapsedTicks);
        }

        public int ParallelF(int W, int max = 0)
        {
            int result = 0;
            if (W > 0)
            {
                int countCPU = 4;
                Task[] tasks = new Task[countCPU];
                for (int j = 0; j < countCPU; j++)
                {
                    tasks[j] = Task.Factory.StartNew(
                        (Object p) =>
                        {
                            var data = p as CustomData;
                            if (data == null)
                            {
                                return;
                            }
                            data.TResult = FPar(W, data.TNum, countCPU);
                        }, new CustomData() { TNum = j });
                }
                Task.WaitAll(tasks);
                for (int i = 0; i < countCPU; i++)
                {
                    if ((tasks[i].AsyncState as CustomData).TResult > result)
                    {
                        result = (tasks[i].AsyncState as CustomData).TResult;
                    }
                }
                return result;
            } else
            {
                return 0;
            }
        }

        public int FPar(int W, int start, int countCPU, int max = 0)
        {
            if (W <= 0)
            {
                return 0;
            }
            for (int i = start; i < n; i += countCPU)
            {
                int tarpinis = FPar(W - w[i], start, countCPU);
                if (w[i] <= W && tarpinis + v[i] >= max)
                {
                    max = tarpinis + v[i];
                }
            }
            return max;
        }
        public int F(int W, int[] w, int[] v, int max = 0)
        {
            if (W <= 0)
            {
                return 0;
            }
            for (int i = 0; i < n; i++)
            {
                int tarpinis = F(W - w[i], w, v, max);
                if (w[i] <= W && tarpinis + v[i] > max)
                {
                    max = tarpinis + v[i];
                }
            }
            return max;
        }
    }
}
