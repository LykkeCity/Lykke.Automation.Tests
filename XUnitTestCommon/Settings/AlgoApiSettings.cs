using Lykke.SettingsReader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Settings.AlgoApi;

namespace XUnitTestCommon.Settings
{
    public class AlgoApiSettings
    {
        public DbSettings Db { get; set; }
        [Optional] public KubernetesSettings Kubernetes { get; set; }
        public ServicesSettings Services { get; set; }
        public TeamCitySettings TeamCity { get; set; }
        public int MaxNumberOfRowsToFetch { get; set; }
    }
}
