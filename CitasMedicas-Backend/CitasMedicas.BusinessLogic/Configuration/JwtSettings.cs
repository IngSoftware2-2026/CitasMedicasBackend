using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.BusinessLogic.Configuration
{
    public class JwtSettings
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public static string Key => _configuration?["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key not configured in appsettings.json");

        public static string Issuer => _configuration?["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer not configured in appsettings.json");

        public static string Audience => _configuration?["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT Audience not configured in appsettings.json");

        public static int ExpirationHours => int.Parse(_configuration?["Jwt:ExpirationHours"] ?? "8");
    }
}
