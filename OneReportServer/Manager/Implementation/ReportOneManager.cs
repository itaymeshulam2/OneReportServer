using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OneReportServer.Client.Interface;
using OneReportServer.Contract.Model;
using OneReportServer.Contract.Request;
using OneReportServer.Contract.Response;
using OneReportServer.DB;
using OneReportServer.DB.Model;
using OneReportServer.DB.Redis;
using OneReportServer.Exceptions;
using OneReportServer.Helper;
using OneReportServer.Manager.Interface;
using OneReportServer.Model;
using OneReportServer.Model.Enum;

namespace OneReportServer.Manager.Implementation
{
    public class ReportOneManager : IReportOne
    {
        private readonly ILogger<ReportOneManager> _logger;
        private readonly AppDBContext _dbContext;


        public ReportOneManager(ILogger<ReportOneManager> logger, AppDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        
        public async Task<GeneralResponse> ReportOne(ReportOneRequest request, int UserId)
        {
            var ReportDetails = new ReportDetailsEntity();
            var report = await _dbContext.ReportDetails.Where(x => x.UserId == UserId && x.Date == DateTime.Today)
                .FirstOrDefaultAsync();
            if (report == null)
            {
                ReportDetails.UserId = UserId;
                ReportDetails.Report = (int)request.RportType;
                ReportDetails.Date = DateTime.Now;
                await _dbContext.ReportDetails.AddAsync(ReportDetails);
                
            }
            else
            {
                report.Report = (int)request.RportType;
                _dbContext.Update(report);
            }
            await _dbContext.SaveChangesAsync();

            return new GeneralResponse
            {
                Success = true
            };
        }

        public async Task<ReportType> GetUserReport(int UserId)
        {
            var res = new ReportType
            {
                Id = 0,
                Value = eRportType.None
            };
            var report = await _dbContext.ReportDetails.Where(x => x.UserId == UserId && x.Date == DateTime.Today)
                .FirstOrDefaultAsync();
            if (report != null)
            {
                res.Id = report.Report;
                res.Value = (eRportType)report.Report;
                res.Name = GetReportTypeName(report.Report);
            }

            return res;
        }

        private string GetReportTypeName(int ReportId)
        {
            Dictionary<int, string> myDictionary = new Dictionary<int, string>();
            myDictionary.Add(1, "ביחידה");
            myDictionary.Add(2, "חו״ל");
            myDictionary.Add(3, "ג");
            myDictionary.Add(4, "חופשה");
            return myDictionary[ReportId];
        }
    }
}