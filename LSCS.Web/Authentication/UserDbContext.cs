using Microsoft.AspNet.Identity.EntityFramework;

namespace LSCS.Web.Authentication
{
    public class UserDbContext : IdentityDbContext<LscsUser>
    {
        public UserDbContext(string connectionStringName) : base(connectionStringName, throwIfV1Schema: false) { }

        public static UserDbContext Create()
        {
            return new UserDbContext("LSCSUserDB");
        }
    }
}