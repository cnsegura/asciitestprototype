using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

using System.Text;

namespace asciitestingNS
{
    class program
    {
        public static void Main()
        {
            PingSpectrumFiveG ping5g = new PingSpectrumFiveG();
            
            bool pingResult = ping5g.PingServer("dns.google.com", 53);
            var dlSpeedtest = DownloadSpeedTest.Download("https://fivegdownloads.blob.core.windows.net/downloadfiles/downloadtest.zip", @"c:\temp\");
            //var dlSpeedtest = DownloadSpeedTest.Download("http://speedtest-ca.turnkeyinternet.net/100mb.bin", @"c:\temp\");

            Console.WriteLine($"Ping result is: {pingResult}");
            Console.WriteLine($"Download Size: {dlSpeedtest.Size} bytes");
            Console.WriteLine($"Time taken: {dlSpeedtest.TimeTaken.TotalSeconds} s");
            Console.WriteLine($"Download speed: {dlSpeedtest.DownloadSpeed, 6:f} Mbps");
            Console.WriteLine($"Parallel processes: {dlSpeedtest.ParallelDownloads}");

            Console.ReadKey();


        }
    }
}
