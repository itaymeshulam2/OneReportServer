namespace OneReportServer.Model
{
    public static class RedisKeys
    {
        private const string OTP_PREFIX = "OTP_";
        private const string AVAILABLE_OTP_PREFIX = "AVAOTP_";

        public static string GetOTPKey(string token)
        {
            return $"{OTP_PREFIX}{token}";
        }

        public static string GetAvailableOTPTOken(string token)
        {
            return $"{AVAILABLE_OTP_PREFIX}{token}";
        }
    }
}
