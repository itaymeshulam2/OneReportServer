namespace OneReportServer.Contract.Response
{
    public class UserAvailableResponse
    {
        public string? AvailableToken { get; set; }
        public string? OTPToken { get; set; }
        public bool IsUserAvailable { get; set; }
    }
}
