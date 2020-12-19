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
        public TimeSpan TimeTaken { get; set; }
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
            }

            WebRequest webRequest = HttpWebRequest.Create(fileUrl);
            webRequest.Method = "HEAD";
            long responseLength;
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                responseLength = long.Parse(webResponse.Headers.Get("Content-Length"));
                result.Size = responseLength;
            }

            using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Append))
            {
                ConcurrentDictionary<int, string> tempFilesDictionary = new ConcurrentDictionary<int, string>();

                List<Range> readRanges = new List<Range>();
                for (int chunk = 0; chunk <numofParallelDownloads -1; chunk++)
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
                    Start = readRanges.Any() ? readRanges.Last().End +1 : 0,
                    End = responseLength -1
                });


                //create and start stopwatch
                var watch = new Stopwatch();
                watch.Start();

                int index = 0;
                Parallel.ForEach(readRanges, new ParallelOptions() { MaxDegreeOfParallelism = numofParallelDownloads }, readRanges =>
                 {
                     HttpWebRequest httpWebRequest = HttpWebRequest.Create(fileUrl) as HttpWebRequest;
                     httpWebRequest.Method = "GET";
                     httpWebRequest.AddRange(readRanges.Start, readRanges.End);
                     using (HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                     {
                         string tempFilePath = Path.GetTempFileName();
                         using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                         {
                             httpWebResponse.GetResponseStream().CopyTo(fileStream);
                             tempFilesDictionary.TryAdd((int)index, tempFilePath);
                         }
                     }
                     index++;
                 });

                result.ParallelDownloads = index;

                watch.Stop();
                
                result.TimeTaken = watch.Elapsed;

                //in Mbps
                result.DownloadSpeed = (result.Size * 8 / 1000000) / result.TimeTaken.TotalSeconds;

                #region Merge to single file
                foreach (var tempFile in tempFilesDictionary.OrderBy(b => b.Key))
                {
                    byte[] tempFileBytes = File.ReadAllBytes(tempFile.Value);
                    destinationStream.Write(tempFileBytes, 0, tempFileBytes.Length);
                    File.Delete(tempFile.Value);
                }
                # endregion

                return result;
            }

        }
    }
}
