using Microsoft.AspNetCore.Mvc;

namespace OneReportServer.Controllers
{
    public class BaseController : ControllerBase
    {

        public BaseController()
        {
        }
        protected int GetUserId()
        {
            var userId = Request.Headers["JWTSub"];
            return int.Parse(userId);
        }
    }
}
