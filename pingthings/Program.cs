using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using CommandLine;
using System.Text;

namespace asciitestingNS
{
    [Verb("scan", HelpText = "Check Internet connectivity by pinging a server with known available port.")]
    class ScanOptions
    {
        [Option('s', "server", Required = true, HelpText = "Server URL to scan.")]
        public string Server { get; set; }

        [Option('p', "port", Required = true, HelpText = "Server port to scan.")]
        public int Port { get; set; }
    }
    [Verb("download", HelpText = "Test download speed.")]
    class DownloadOptions
    {
        [Option('u', "url", Required = true, HelpText = "URL do download from.")]
        public string DownloadUrl { get; set; }

        [Option('s', "save", Required = false, Default = ".", HelpText = "Local directory to save file (default is .)")]
        public string SaveLocation { get; set; }
    }


    class program
    {
        static int Main(string[] args)
        {

            return CommandLine.Parser.Default.ParseArguments<ScanOptions, DownloadOptions>(args)
            .MapResult(
                (ScanOptions opts) => RunScanTest(opts),
                (DownloadOptions opts) => RunDownloadTest(opts),
                e => 1);
        }
        
        static int RunScanTest(ScanOptions opts)
        {

            var pingResult = PingSpectrumFiveG.PingServer(opts.Server, opts.Port);
            if(pingResult != null) 
            {
                Console.WriteLine($"Ping result is: {pingResult.PingSuccess}");
                Console.WriteLine($"Average response time is: {pingResult.AveragePingTimeMs} ms");
                Console.WriteLine($"Max response time is: {pingResult.MaxPingTimeMs} ms");
                Console.WriteLine($"Min response time is: {pingResult.MinPingTimeMs} ms");
                Console.WriteLine($"Response standard deviation (p) is {pingResult.PingStandardDeviationS}");
                return 1; 
            } //fix 
            else 
            {
                Console.WriteLine($"Ping result is: {false}");
                return 0;  
            }
        }
        private static int RunDownloadTest(DownloadOptions opts)
        {
            //suggested download files:
            //https://fivegdownloads.blob.core.windows.net/downloadfiles/downloadtest.zip
            //https://fivegdownloads.blob.core.windows.net/downloadfiles/bigJsonFile.json
            //http://speedtest-ca.turnkeyinternet.net/100mb.bin

            var dlSpeedtest = DownloadSpeedTest.Download(opts.DownloadUrl, opts.SaveLocation);
            if (dlSpeedtest != null)
            {
                Console.WriteLine($"Download Size: {dlSpeedtest.Size} bytes");
                Console.WriteLine($"Time taken: {dlSpeedtest.TimeTaken,6:f} s");
                Console.WriteLine($"Download speed: {dlSpeedtest.DownloadSpeed,6:f} Mbps");
                Console.WriteLine($"Parallel processes: {dlSpeedtest.ParallelDownloads}");
                return 1;
            }
            else
            {
                Console.WriteLine($"Download failed");
                return 0;
            }
        }

    }
}

