using System.Text;
using System.Text.RegularExpressions;

namespace OneReportServer.Helper
{
    public class GeneralHelper
    {
        static Random rd = new Random();

        public static TEnum GetEnumValue<TEnum>(string value, TEnum defaultValue = default) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out TEnum result) ? result : defaultValue;
        }

        public static string GetBasePathLocation(string subFolder = null, bool shouldCreateFolder = true)
        {
            var res = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subFolder ?? "");
            if (shouldCreateFolder && !Directory.Exists(res))
            {
                Directory.CreateDirectory(res);
            }
            return res;
        }

        public static string GetPath(string baseFolder, string subFolder)
        {
            var res = Path.Combine(baseFolder, subFolder ?? "");
            if (!Directory.Exists(res))
            {
                Directory.CreateDirectory(res);
            }
            return res;
        }

        public static string GetSha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string GetRandomToken()
        {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var resultToken = new string(
               Enumerable.Repeat(allChar, 32)
               .Select(token => token[rd.Next(token.Length)]).ToArray());

            return resultToken.ToString();
        }
        
        public static string CreateRandomNumber(int stringLength)
        {
            const string allowedChars = "0123456789";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public static string GetRandom6Digits()
        {
            var x = rd.Next(0, 1000000);
            return x.ToString("000000");
        }

        
        public static bool IsValidPhone(string? Email)
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                    return false;
                var r = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return r.IsMatch(Email);

            }
            catch (Exception)
            {
                return false; ;
            }
        }
    }
}