using System.ComponentModel.DataAnnotations.Schema;

namespace OneReportServer.DB.Model;

[Table("report_one")]
public class ReportDetailsEntity
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int Report { get; set; }
    public DateTime Date { get; set; }
}