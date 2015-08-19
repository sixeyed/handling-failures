using System.Configuration;

namespace Sixeyed.HandlingFailures.Core
{
    public static class Config
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
