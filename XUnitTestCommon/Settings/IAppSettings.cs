using Lykke.SettingsReader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.Settings
{
    public interface IAppSettings
    {
        String UrlPefix { get; set; }
        String BaseUrl { get; set; }
        int AuthTokenTimeout { get; set; }
        String AuthPath { get; set; }
        String RegisterPath { get; set; }
        String BaseUrlAuth { get; set; }
        String BaseUrlRegister { get; set; }
        String AuthEmail { get; set; }
        String AuthPassword { get; set; }
        bool IsHttps { get; set; }
    }
}
