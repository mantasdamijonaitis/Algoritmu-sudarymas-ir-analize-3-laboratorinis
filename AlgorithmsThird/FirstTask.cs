using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsThird
{
    class FirstTask
    {

        private const int DataAmount = 10000000;
        private const int DataLength = 25;

        public void SequentialSearch()
        {
            LinearHashTable<int, string> hashTable = new LinearHashTable<int, string>();
            for (int i = 0; i < DataAmount; i++)
            {
                string randomString = Guid.NewGuid().ToString("n").Substring(0, DataLength);
                hashTable.Add(i, randomString);
            }
            int count = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < DataLength; i++)
            {
                string value = string.Empty;
                if (hashTable[i] != null)
                {
                    count++;
                }
            }
            stopWatch.Stop();
            Console.WriteLine("neislygiagretintas. Elementu sk: {0} Laikas: {1}", DataAmount, stopWatch.ElapsedTicks);
        }

        public void ParallelSearch()
        {
            LinearHashTable<int, string> hashTable = new LinearHashTable<int, string>();
            for (int i = 0; i < DataAmount; i++)
            {
                string randomString = Guid.NewGuid().ToString("n").Substring(0, DataLength);
                hashTable.Add(i, randomString);
            }
            int cpuCount = 8;
            Task<int>[] tasks = new Task<int>[cpuCount];
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int j = 0; j < cpuCount; j++)
            {
                tasks[j] = Task<int>.Factory.StartNew(
                    (object p) =>
                    {
                        int count = 0;
                        for (int i = (int)p; i < DataAmount; i += cpuCount)
                        {
                            string val = string.Empty;
                            if (hashTable[i] != null)
                            {
                                count++;
                            }
                        }
                        return count;
                    }, j);
            }
            int total = 0;
            for (int i = 0; i < cpuCount; i++)
            {
                total += tasks[i].Result;
            }
            stopWatch.Stop();
            Console.WriteLine("islygiagretintas. Elementu sk: {0} Laikas: {1}", DataAmount, stopWatch.ElapsedTicks);
        }
    }

}
