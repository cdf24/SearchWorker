using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SearchWorker
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration = null;
        public static IConfiguration Instance {
            get 
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsetting.json").Build();
                }

                return _configuration;
            }
        }
    }
}
