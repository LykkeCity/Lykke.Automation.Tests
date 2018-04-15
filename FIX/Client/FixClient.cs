using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using Autofac;
using Common;
using Common.Log;
using FIX.Client;
//using Lykke.Logging;
//using Lykke.Service.FixGateway.Core.Services;
//using Lykke.Service.FixGateway.Core.Settings.ServiceSettings;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using QuickFix.Lykke;
using QuickFix.Transport;
using XUnitTestCommon.TestsCore;
using Message = QuickFix.Message;


namespace FIX.Client
{
    public class FixClient : IApplication,/* ISupportInit,*/ IStopable
    {
        private readonly SocketInitiator _socketInitiator;
        private SessionID _sessionId;
        private readonly LogToConsole _log;
        private readonly BlockingCollection<Message> _appMessages = new BlockingCollection<Message>(1);
        private readonly BlockingCollection<Message> _adminMessages = new BlockingCollection<Message>(1);

        public FixClient(string targetCompId = Const.TargetCompId, string senderCompId = Const.SenderCompId, string uri = Const.Uri, int port = Const.Port)
        {
            var s = new SessionSetting
            {
                TargetCompID = targetCompId,
                SenderCompID = senderCompId,
                FixConfiguration = new[]
                {
                    "[DEFAULT]",
                    "ResetOnLogon=Y",
                    "FileStorePath=client",
                    "ConnectionType=initiator",
                    "ReconnectInterval=60",
                    "BeginString=FIX.4.4",
                    @"DataDictionary=FIX/ClientFIX44.xml",
                    "SSLEnable=N",
                    @"SSLProtocols=Tls",
                    "SSLValidateCertificates=N",
                    $"SocketConnectPort={port}",
                    "StartTime=00:00:00",
                    "EndTime=00:00:00",
                    "HeartBtInt=10",
                    "LogonTimeout=120",
                    $"SocketConnectHost={uri}",
                    "[SESSION]",
                }
            };
            _log = new LogToConsole();
            var settings = new SessionSettings(s.GetFixConfigAsReader());
            var storeFactory = new MemoryStoreFactory();
            var logFactory = new LykkeLogFactory(_log, true, true, true);
            _socketInitiator = new SocketInitiator(this, storeFactory, settings, logFactory);
        }

        public void ToAdmin(Message message, SessionID sessionID)
        {
            _log.WriteInfo("FixClient", "ToAdmin", "");
            if (message is Logon logon)
            {
                logon.Password = new Password(Environment.GetEnvironmentVariable("FIXWrongPassword")??  Const.Password);
            }
        }

        public void FromAdmin(Message message, SessionID sessionID)
        {
            _log.WriteInfo("FixClient", "FromAdmin", "");
            if (message is TestRequest || message is Heartbeat)
            {
                return;
            }
            _adminMessages.Add(message);
        }

        public void ToApp(Message message, SessionID sessionId)
        {
           // _log.WriteInfo("FixClient", "ToApp", "");
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            //_log.WriteInfo("FixClient", "FromApp", "");
            _appMessages.Add(message);
        }

        public void OnCreate(SessionID sessionID)
        {
            _log.WriteInfo("FixClient", "OnCreate", "");
            _sessionId = sessionID;
        }

        public void OnLogout(SessionID sessionID)
        {
            _log.WriteInfo("FixClient", "OnLogout", "");
        }

        public void OnLogon(SessionID sessionID)
        {
            _log.WriteInfo("FixClient", "OnLogon", "");
        }

        public void Send(Message message)
        {
            var header = message.Header;
            header.SetField(new SenderCompID(_sessionId.SenderCompID));
            header.SetField(new TargetCompID(_sessionId.TargetCompID));
            Log(message);

            var result = QuickFix.Session.SendToTarget(message);
            if (!result)
            {
                throw new InvalidOperationException("Unable to send request. Unknown error");
            }
        }

        public T GetResponse<T>(int timeout = 20000) where T : Message
        {
            _appMessages.TryTake(out var message, TimeSpan.FromMilliseconds(timeout));
            if ((T)message == null)
            {
                var admMessage = GetAdminResponse<T>();
                Log(admMessage, "Recieving adming message");
            }
                
            Log(message, "Recieving");

            return (T)message;
        }

        public T GetAdminResponse<T>(int timeout = 2000) where T : Message
        {
            _adminMessages.TryTake(out var message, TimeSpan.FromMilliseconds(timeout));
            return (T)message;
        }

        public void Init()
        {
            _socketInitiator.Start();
            for (var i = 0; i < 1000; i++)
            {
                if (_socketInitiator.IsLoggedOn)
                {
                    return;
                }
                Thread.Sleep(5);
            }
        }

        public void Dispose()
        {
            _socketInitiator.Dispose();
        }

        public void Stop()
        {
            _socketInitiator.Stop();
            for (var i = 0; i < 10; i++)
            {
                if (!_socketInitiator.IsLoggedOn)
                {
                    return;
                }
                Thread.Sleep(500);
            }
        }

        #region allure report region
        private void Log(Message message, string send = "Sending")
        {
            string attachName = $"{send} message";
            var attachContext = new StringBuilder();
            attachContext.AppendLine(send);
            if (message != null)
            {
                attachContext.AppendLine(message.ToString().Replace("\u0001", "|"));
            }
            Allure2Helper.AttachJson(attachName, attachContext.ToString());
        }

        #endregion

    }
}