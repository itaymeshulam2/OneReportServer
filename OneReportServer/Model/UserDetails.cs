
using OneReportServer.Model.Enum;

namespace OneReportServer.Model
{
    public class UserDetails
    {
        public long UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public eRoleType Role { get; set; } = eRoleType.User; }
}
