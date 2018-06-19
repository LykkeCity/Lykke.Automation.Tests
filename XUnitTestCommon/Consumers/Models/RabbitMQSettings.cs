using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;

namespace XUnitTestCommon.Consumers.Models
{
    public class RabbitMQSettings
    {
        public RabbitMQSettings(string hostname, string port, string username, string password, string sourceEndpoint, string endpoint)
        {
            Hostname = hostname;
            Port = port;
            Username = username;
            Password = password;
            SourceEndpoint = sourceEndpoint;
            Endpoint = endpoint;
        }

        public RabbitMQSettings(MatchingEngineSettings config, string sourceEndpoint, string endpoint)
        {
            Hostname = config.RabbitMQHost;
            Port = config.RabbitMQamqpPort;
            Username = config.RabbitMQUsername;
            Password = config.RabbitMQPassword;
            SourceEndpoint = sourceEndpoint;
            Endpoint = endpoint;
        }

        public string Hostname { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string SourceEndpoint { get; set; }
        public string Endpoint { get; set; }
    }
}
