using Microsoft.Extensions.Configuration;
using System.IO;

namespace Matchmaker.Configuration
{
    public static class ConfigurationLoader
    {
        public static AppSettings LoadConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configuration.json", optional: false, reloadOnChange: true)
                .Build();

            var config = new AppSettings();
            configuration.Bind(config);
            return config;
        }
    }
}
