using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSE.Core.Utils
{
    public static class ConfigurationExtensions
    {
        public static string GetMessageQueuConnection(this IConfiguration configuration, string name)
        {
            return configuration?.GetSection("MessageQeueConnection")?[name];
        }
    }
}
