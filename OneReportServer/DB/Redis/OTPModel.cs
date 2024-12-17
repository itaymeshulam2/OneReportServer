namespace OneReportServer.DB.Redis
{
    public class OTPModel
    {
        public string Email { get; set; }
        public string OTPToken { get; set; }
        public string OTPCode { get; set; }
        public string? AvailableToken { get; set; }
        public bool IsUserPhoneIsValid { get; set; }
    }
}
