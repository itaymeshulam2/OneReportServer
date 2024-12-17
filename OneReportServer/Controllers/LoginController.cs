using OneReportServer.Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using OneReportServer.Contract.Request;
using OneReportServer.Contract.Response;
using OneReportServer.Manager.Interface;

namespace OneReportServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginManager _loginManager;


        public LoginController(ILogger<LoginController> logger, ILoginManager loginManager) : base()
        {
            _logger = logger;
            _loginManager = loginManager;
        }

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<LoginResponse> PostLogin(LoginRequest request)
        {
            return await _loginManager.Login(request);
        }

        [AllowAnonymous]
        [HttpPost("is-user-available")]
        public async Task<UserAvailableResponse> IsUserAvailable(UserAvailableRequest request)
        {
            return await _loginManager.IsUserAvailable(request);
        }
    }
}