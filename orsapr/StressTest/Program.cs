using System;
using System.Diagnostics;
using System.Management;
using Logic;
using API_singly;
using System.IO;

namespace StressTest
{
    /// <summary>
    /// Класс для проведения стресс-тестов
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Проведение стресс-тестов
        /// </summary>
        static void Main(string[] args)
        {
            string logPath = @"..\\..\\..\\..\\docs\\log.txt";
            if (File.Exists(logPath))
            {
                File.Delete(logPath);
            }

            File.Create(logPath).Close();
            StreamWriter writer = new StreamWriter(logPath);

            Parameters parameters = new Parameters();
            parameters.SeatWidth = 300;
            parameters.SeatLength = 300;
            parameters.SeatThickness = 30;
            parameters.LegLength = 300;
            parameters.LegWidth = 30;
            parameters.SeatType = SeatTypes.SquareSeat;
            parameters.LegsType = LegTypes.RoundLeg;

            var builder = new Builder();
            var stopWatch = new Stopwatch();
            var count = 0;

            while (true)
            {
                ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
                ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                var enumerator = managementObjectCollection.GetEnumerator();
                enumerator.MoveNext();
                var managementObject = enumerator.Current;

                stopWatch.Start();
                builder.Build(parameters);
                stopWatch.Stop();
                var totalMemory = double.Parse(managementObject["TotalVisibleMemorySize"].ToString()) / 1024 / 1024;
                var freeMemory = double.Parse(managementObject["FreePhysicalMemory"].ToString()) / 1024 / 1024;
                var usedMemory = (totalMemory - freeMemory);
                writer.WriteLine($"{++count}\t{stopWatch.Elapsed:hh\\:mm\\:ss}\t{usedMemory}");
                writer.Flush();
                stopWatch.Reset();
            }
            {
                ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);
                ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                var enumerator = managementObjectCollection.GetEnumerator();
                enumerator.MoveNext();
                var managementObject = enumerator.Current;
                writer.WriteLine($"End {double.Parse(managementObject["TotalVisibleMemorySize"].ToString()) / 1024 / 1024}");
            }
        }
    }
}
