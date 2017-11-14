using System;
using System.Collections.Generic;
using System.Text;

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

        public RabbitMQSettings(ConfigBuilder config, string sourceEndpoint, string endpoint)
        {
            Hostname = config.Config["RabbitMQHost"];
            Port = config.Config["RabbitMQHost"];
            Username = config.Config["RabbitMQUsername"];
            Password = config.Config["RabbitMQPassword"];
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
