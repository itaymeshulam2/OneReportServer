using OneReportServer.DB.Redis;

namespace OneReportServer.Client.Interface
{
    public interface IRedisClient
    {
        public Task SetOTPToken(OTPModel otpModel);
        Task SetOTPAvailableToken(OTPModel otpModel);
        public Task<OTPModel> GetOTPToken(string token);

    }
}
