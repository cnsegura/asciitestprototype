using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace asciitestingNS
{
    public class PingSpectrumFiveG
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
            catch (SocketException ex)
            {
                return false;
            }

        }

    }
}
