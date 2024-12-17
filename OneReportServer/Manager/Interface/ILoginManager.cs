
using OneReportServer.Contract.Request;
using OneReportServer.Contract.Response;

namespace OneReportServer.Manager.Interface
{
    public interface ILoginManager
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<UserAvailableResponse> IsUserAvailable(UserAvailableRequest request);
    }
}
