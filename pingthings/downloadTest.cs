using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace asciitestingNS
{
    internal class Range
    {
        public long Start { get; set; }
        public long End { get; set; }
    }

    public class DownloadResult
    {
        public long Size { get; set; }
        public string FilePath { get; set; }
        public double TimeTaken { get; set; }
        public int ParallelDownloads { get; set; }
        public double DownloadSpeed { get; set; }
    }

    public static class DownloadSpeedTest
    {
        static DownloadSpeedTest()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.MaxServicePointIdleTime = 1000;
        }

        public static DownloadResult Download(string fileUrl, string destinationFolder, int numofParallelDownloads = 0)
        {
            Uri uri = new Uri(fileUrl);

            //do I even need a destination path?
            string destinationFilePath = Path.Combine(destinationFolder, uri.Segments.Last());

            DownloadResult result = new DownloadResult() { FilePath = destinationFilePath };

            //create parallel downloads
            if (numofParallelDownloads <=0 )
            {
                numofParallelDownloads = Environment.ProcessorCount;

                //test of setting num'odownloads to -1
                //numofParallelDownloads = 1;
            }


            WebRequest webRequest = HttpWebRequest.Create(fileUrl);
            webRequest.Method = "HEAD";
            long responseLength;
            try
            {
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    responseLength = long.Parse(webResponse.Headers.Get("Content-Length"));
                    result.Size = responseLength;
                }
                using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Append))
                {
                    ConcurrentDictionary<int, string> tempFilesDictionary = new ConcurrentDictionary<int, string>();

                    List<Range> readRanges = new List<Range>();
                    for (int chunk = 0; chunk < numofParallelDownloads - 1; chunk++)
                    {
                        var range = new Range()
                        {
                            Start = chunk * (responseLength / numofParallelDownloads),
                            End = ((chunk + 1) * (responseLength / numofParallelDownloads)) - 1
                        };
                        readRanges.Add(range);
                    }

                    readRanges.Add(new Range()
                    {
                        Start = readRanges.Any() ? readRanges.Last().End + 1 : 0,
                        End = responseLength - 1
                    });

                    int index = 0;
                    long threadElapsedTicks = 0;
                    Parallel.ForEach(readRanges, new ParallelOptions() { MaxDegreeOfParallelism = numofParallelDownloads }, readRanges =>
                     {
                         //make this run as fast as possible
                         var threadPrio = Thread.CurrentThread.Priority;
                         Thread.CurrentThread.Priority = ThreadPriority.Highest;

                         HttpWebRequest httpWebRequest = HttpWebRequest.Create(fileUrl) as HttpWebRequest;
                         httpWebRequest.Method = "GET";
                         httpWebRequest.AddRange(readRanges.Start, readRanges.End);
                         using (HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                         {
                             long contentLength = httpWebResponse.ContentLength;
                             byte[] responseBytes = new byte[contentLength];
                             int count;

                             Stream streamReader = httpWebResponse.GetResponseStream();
                             BinaryReader binReader = new BinaryReader(streamReader);
                             MemoryStream ms = new MemoryStream();
                             Stopwatch watch = new Stopwatch();

                             watch.Start();
                             while ((count = binReader.Read(responseBytes, 0, responseBytes.Length)) != 0)
                             { 
                                 //This loop runs until all bytes sent by server have been read
                                 //We don't do anything w/ the data as we're just interested
                                 //in download speed, not processing

                                 //code below for debug

                                 //ms.Write(responseBytes, 0, count);
                                //Console.WriteLine(@"After: responseBytes = {0}, totalRead = {1} ", responseBytes.Length, count);
                             }
                             watch.Stop();
                             

                             //because types for Interlocked.Add and the Stopwatch don't match well, we'll capture last
                             //finishing threads ticks and pass that out
                             long elapsedTicks = watch.ElapsedTicks;
                             Interlocked.Exchange(ref threadElapsedTicks, elapsedTicks);
                             
                             //debug
                             Console.WriteLine("elapsedTicks for Thread {0} was {1}, readRange.Start was = {2}, readRange.End was = {3}", Thread.CurrentThread.ManagedThreadId, elapsedTicks, readRanges.Start, readRanges.End);
                             
                             watch.Reset();
                             Thread.CurrentThread.Priority = threadPrio;
                         }
                         index++;
                         
                     });
                    
                    result.ParallelDownloads = index;

                    //calculate elapsed time from elapsed ticks in seconds
                    long stopwatchFrequency = Stopwatch.Frequency;
                    result.TimeTaken = (double)threadElapsedTicks / stopwatchFrequency;

                    //debug
                    Console.WriteLine("threadElapsedTicks for was = {0}", threadElapsedTicks);
                    //in Mbps
                    result.DownloadSpeed = (result.Size * 8 / 1000000) / result.TimeTaken;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Download test failed {0}", ex);
                return null;
            }
        }
    }
}
