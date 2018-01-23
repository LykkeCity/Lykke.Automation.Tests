using System;
using System.Collections.Generic;
using System.Text;

namespace LykkeAutomationPrivate
{
    public static class EnvConfig
    {
        //TODO: Add enviroment var to TC config, rename maybe
        public static Env Env => Environment.GetEnvironmentVariable("ATEnv") == "TEST" ? Env.Test : Env.Dev;
    }

    public enum Env { Dev, Test }
}
