using Microsoft.Extensions.Configuration;
using System.IO;

namespace SqlOnlyMethodTests
{
    public class ConfigurationHelper
    {
        private static IConfigurationRoot _Configuration;
        public static IConfigurationRoot Configuration
        {
            get
            {
                if ( _Configuration == null )
                {
                    IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath( Directory.GetCurrentDirectory().ToString() )
                    .AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true )
                    .AddEnvironmentVariables();
                    
                    _Configuration = builder.Build();
                }

                return _Configuration;
            }
        }
    }
}