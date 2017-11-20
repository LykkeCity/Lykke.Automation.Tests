using System;
using System.Collections.Generic;
using System.Text;

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

        public RabbitMQHttpApiSettings(ConfigBuilder config)
        {
            Hostname = config.Config["RabbitMQHost"];
            Port = config.Config["RabbitMQPort"];
            Username = config.Config["RabbitMQUsername"];
            Password = config.Config["RabbitMQPassword"];
            Node = config.Config["RabbitMQNode"];
        }

        public string Hostname { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Node { get; set; }
    }
}
