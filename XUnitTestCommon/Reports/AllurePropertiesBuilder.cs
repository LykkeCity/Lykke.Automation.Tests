using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestsCore.TestsCore
{
    public class AllurePropertiesBuilder
    {
        private static object _lock = new object();
        private static AllurePropertiesBuilder _intance;
        private static List<string> properties = new List<string>();

        private AllurePropertiesBuilder() { }

        public static AllurePropertiesBuilder Instance
        {
            get
            {
                if (_intance == null)
                    lock (_lock)
                    {
                        _intance = new AllurePropertiesBuilder();
                    }
                return _intance;
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
            File.WriteAllLines(pathToSave, properties.ToArray());
        }
    }
}
