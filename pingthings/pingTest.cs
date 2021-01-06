using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
using MathNet.Numerics.Statistics;
using System.Linq;

namespace asciitestingNS
{
    public class PingSpectrumFiveG
    {
        private class PinginMs
        {
            public int Iteration { get; set; }
            public long PingMs { get; set; }
        }
        public class PingResult
        {
            public double AveragePingTimeMs { get; set; }
            public bool PingSuccess { get; set; }
            public double PingStandardDeviationS { get; set; }
            public double MinPingTimeMs { get; set; }
            public double MaxPingTimeMs { get; set; }

        }

        public static PingResult PingServer(string server, Int32 port)
        {

            PingResult result = new PingResult();

            try
            {
                
                IPAddress ipAddress = System.Net.IPAddress.Parse(server);
                
                Stopwatch watch = new Stopwatch();

                var pingtimes = new List<Tuple<int, long>>();

                for (int i = 0; i < 5; i++)
                {

                    TcpClient tcpClient = new TcpClient();
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port); 
                    watch.Start();
                    //method blocks until connection is made or fails
                    tcpClient.Connect(ipEndPoint);
                    watch.Stop();
                    tcpClient.Close();
                    long elapsedTimeMs = watch.ElapsedMilliseconds;
                    //pingtimes.Add(new PinginMs() { Iteration = i, PingMs = elapsedTimeMs });
                    pingtimes.Add(new Tuple<int, long>(i, elapsedTimeMs ));

                    //debug
                    //Console.WriteLine("iteration {0} took {1} ms", i, elapsedTimeMs);

                    watch.Reset();

                }

                var stdDev = pingtimes.Select(x => (double)x.Item2).MeanStandardDeviation();
                var max = pingtimes.Select(x => (double)x.Item2).Max();
                var min = pingtimes.Select(x => (double)x.Item2).Min();
                var average = pingtimes.Select(x => (double)x.Item2).Average();

                result.AveragePingTimeMs = stdDev.Item1;
                result.PingStandardDeviationS = stdDev.Item2;
                result.MinPingTimeMs = min;
                result.MaxPingTimeMs = max;
                result.PingSuccess = true;

                return result;

            }
            catch (SocketException ex)
            {
                Console.WriteLine("Connection check failed {0}", ex);
                return null; 
            }

        }

    }
}
