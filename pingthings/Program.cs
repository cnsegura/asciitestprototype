using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace asciitestingNS
{
    public class SpectrumFiveG
    {
        
        public Boolean PingServer(string server, Int32 port)
        {
            Int32 porttotry = port;
            string servertotry = server;
            try
            {
                TcpClient client = new TcpClient(servertotry, porttotry);
                return true;
            }
            catch(SocketException ex)
            {
                return false;
            }

        }

        public static void Main()
        {
            SpectrumFiveG sp = new SpectrumFiveG();
            sp.PingServer("dns.google.com", 53);

        }
    }
}
