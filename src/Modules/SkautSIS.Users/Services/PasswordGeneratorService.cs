using System.Web.Security;

namespace SkautSIS.Users.Services
{
    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        public string Generate()
        {
            return Membership.GeneratePassword(10, 5);
        }
    }
}