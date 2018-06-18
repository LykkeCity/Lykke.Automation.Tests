using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;

namespace XUnitTestCommon.Consumers.Models
{
    public class RabbitMQHttpApiSettings
    {
        public RabbitMQHttpApiSettings(string hostname, string port, string username, string password, string node)
        {
            Hostname = hostname;
            Port = port;
            Username = username;
            Password = password;
            Node = node;
        }

        public RabbitMQHttpApiSettings(MatchingEngineSettings config)
        {
            Hostname = config.RabbitMQHost;
            Port = config.RabbitMQPort;
            Username = config.RabbitMQUsername;
            Password = config.RabbitMQPassword;
            Node = config.RabbitMQNode;
        }

        public string Hostname { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Node { get; set; }
    }
}
