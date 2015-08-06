using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace LSCS.Web.Authentication
{
    public class ApplicationSignInManager : SignInManager<LscsUser, string>
    {
        public ApplicationSignInManager(LscsUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(LscsUser user)
        {
            return ((LscsUserManager)UserManager).CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
        }
    }
}