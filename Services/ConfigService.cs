using Microsoft.Extensions.Configuration;

namespace SubaruBOT.Services
{
    public class ConfigService
    {
        private static IConfiguration? _configuration;

        public static IConfiguration LoadConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            }

            return _configuration;
        }

        public static string GetConnectionString(string name = "DefaultConnection")
        {
            var config = LoadConfiguration();
            return config.GetConnectionString(name) ?? throw new InvalidOperationException($"Connection string '{name}' not found.");
        }
    }
}