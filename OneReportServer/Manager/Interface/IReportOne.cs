using OneReportServer.Contract.Model;
using OneReportServer.Contract.Request;
using OneReportServer.Contract.Response;

namespace OneReportServer.Manager.Interface;

public interface IReportOne
{
    Task<GeneralResponse> ReportOne(ReportOneRequest request, int UserId);
    Task<ReportType> GetUserReport(int UserId);

}