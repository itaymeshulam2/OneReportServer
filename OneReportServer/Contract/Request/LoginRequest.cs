namespace OneReportServer.Contract.Request
{
    public class LoginRequest : BaseRequest
    {
        public string Email { get; set; }
        public string OTPToken { get; set; }
        public string OTPCode { get; set; }
        public string? AvailableToken { get; set; }
    }
}
