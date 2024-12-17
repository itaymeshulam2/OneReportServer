using OneReportServer.Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using OneReportServer.Contract.Model;
using OneReportServer.Contract.Request;
using OneReportServer.Contract.Response;

namespace OneReportServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportOneController : BaseController
    {
        private readonly ILogger<ReportOneController> _logger;
        private readonly IReportOne _reportOne;


        public ReportOneController(ILogger<ReportOneController> logger, IReportOne reportOne) : base()
        {
            _logger = logger;
            _reportOne = reportOne;
        }

        [Authorize]
        [HttpPost("report-one")]
        public async Task<GeneralResponse> ReportOne(ReportOneRequest request)
        {
            return await _reportOne.ReportOne(request, GetUserId());
        }
        
        [Authorize]
        [HttpGet("user/report/get")]
        public async Task<ReportType> GetUserReport()
        {
            return await _reportOne.GetUserReport(GetUserId());
        }
        
        [Authorize]
        [HttpGet("report/type/get")]
        public GetReportTypeResponse GetReportType()
        {
            return new GetReportTypeResponse
            {
                Details = new List<ReportType>
                {
                    new ReportType
                    {
                        Id = 1,
                        Value = eRportType.In, 
                        Name = "ביחידה"
                    },
                    new ReportType
                    {
                        Id = 2,
                        Value = eRportType.Abroad, 
                        Name = "חו״ל"
                    },
                    new ReportType
                    {
                        Id = 3,
                        Value = eRportType.Sick, 
                        Name = "ג"
                    },
                    new ReportType
                    {
                        Id = 4,
                        Value = eRportType.Freedom, 
                        Name = "חופשה"
                    }
                },
     
            }; 
        }
    }
}