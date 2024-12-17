using System.ComponentModel.DataAnnotations.Schema;
using OneReportServer.Model.Enum;

namespace OneReportServer.DB.Model;

[Table("user_details")]
public class UserDetailsEntity
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public eRoleType Role { get; set; } = eRoleType.User;
}