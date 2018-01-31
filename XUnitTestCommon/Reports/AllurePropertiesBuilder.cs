using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XUnitTestCommon.TestsCore
{
    public class AllurePropertiesBuilder
    {
        private static object _lock = new object();
        private static object _envLock = new object();
        private static Lazy<AllurePropertiesBuilder> _intance;
        private static HashSet<string> properties = new HashSet<string>();

        private AllurePropertiesBuilder() { }

        public static AllurePropertiesBuilder Instance
        {
            get
            {
                    if (_intance == null)
                        _intance = new Lazy<AllurePropertiesBuilder>(new AllurePropertiesBuilder());

                return _intance.Value;
            }
        }

        public void AddPropertyPair(string name, string value)
        {
            lock (_lock)
            {
                properties.Add($"{name}={value}");
            }
        }

        public void SaveAllureProperties(string pathToSave)
        {
            int i = 1;
            var finalProperties = new List<string>();
            properties.ToList().ForEach(p => 
            {
                if (p.Contains("service="))
                    finalProperties.Add(p.Replace("service=", $"service{i++}="));
                else
                    finalProperties.Add(p);
            });
            lock (_envLock)
            {
                File.AppendAllLines(pathToSave, finalProperties.ToList());
            }
        }
    }
}
