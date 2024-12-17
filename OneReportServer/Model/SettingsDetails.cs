using Serilog;

namespace OneReportServer.Model
{
    public class SettingsDetails
    {

        public static void LoadAllSettings()
        {
            //TODO fix the env
            Log.Information("Load SettingsDetails");
            var a = JWTTokenPrivateKey;
            a = DBConnectionString;
            a = RedisHost;
            a = RedisPassword;
            a = EmailPassword;
            var b = JWTExpireIn;
            Log.Information("Done Load SettingsDetails");
        }

        public const string DATE_FORMAT_SHORT = "yyyy-MM-dd";
        public const string DATE_FORMAT_LONG = "yyyy-MM-dd HH:mm:ss";

        private static string _DBConnectionString;
        public static string DBConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_DBConnectionString))
                {
                    _DBConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
                    Log.Information($"_DBConnectionString Length: [{_DBConnectionString.Length}]");
                }
                return _DBConnectionString;
            }
        }

        private static string _JWTTokenPrivateKey;
        public static string JWTTokenPrivateKey
        {
            get
            {
                if (string.IsNullOrEmpty(_JWTTokenPrivateKey))
                {
                    _JWTTokenPrivateKey = Environment.GetEnvironmentVariable("JWT_TOKEN_PRIVATE_KEY");
                    Log.Information($"_JWTTokenPrivateKey Length: [{_JWTTokenPrivateKey.Length}]");
                }
                return _JWTTokenPrivateKey;
            }
        }

        private static int? _JWTExpireIn;
        public static int JWTExpireIn
        {
            get
            {
                if (!_JWTExpireIn.HasValue)
                {
                    _JWTExpireIn = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE_IN"));
                    Log.Information($"_JWTExpireIn Length: [{_JWTExpireIn}]");
                }
                return _JWTExpireIn.Value;
            }
        }

        // Redis
        private static string _RedisHost;
        public static string RedisHost
        {
            get
            {
                if (string.IsNullOrEmpty(_RedisHost))
                {
                    _RedisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
                    Log.Information($"_RedisHost: [{_RedisHost.Length}]");
                }
                return _RedisHost;
            }
        }

        private static int? _RedisPort;
        public static int RedisPort
        {
            get
            {
                if (!_RedisPort.HasValue)
                {
                    _RedisPort = int.Parse(Environment.GetEnvironmentVariable("REDIS_PORT"));
                    Log.Information($"_RedisPort: [{_RedisPort}]");
                }
                return _RedisPort.Value;
            }
        }

        private static string _RedisPassword;
        public static string RedisPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_RedisPassword))
                {
                    _RedisPassword = "JSfsgdO53LrC6xtMfydbfQ2FEo4Ob7rfILNVRwDV";
                        Environment.GetEnvironmentVariable("REDIS_PASSWORD");
                    Log.Information($"_RedisPassword: [{_RedisPassword.Length}]");
                }
                return _RedisPassword;
            }
        }

        private static string _EmailPassword;
        public static string EmailPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_EmailPassword))
                {
                    _EmailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
                    Log.Information($"EmailPassword: [{_EmailPassword}]");
                }
                return _EmailPassword;
            }
        }
    }
}
