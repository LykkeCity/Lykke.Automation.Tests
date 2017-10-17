using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Lykke.MatchingEngine.Connector.Services;

namespace XUnitTestCommon.Consumers
{
    public class MatchingEngineConsumer
    {
        private readonly string _hostName;
        private readonly int _port;

        private TcpMatchingEngineClient _client;
        public TcpMatchingEngineClient Client { get { return _client; } }

        public MatchingEngineConsumer(string hostName, int port)
        {
            _hostName = hostName;
            _port = port;

            this.Connect();
        }

        public void Connect()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(_hostName);
            if (hostEntry.AddressList.Length > 0)
            {
                IPEndPoint remoteEndpoint = new IPEndPoint(hostEntry.AddressList[0], _port);
                _client = new TcpMatchingEngineClient(remoteEndpoint);
                _client.Start();
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
