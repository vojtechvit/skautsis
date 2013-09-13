using Orchard.Security;

namespace SkautSIS.Users.Services
{
    public class UsernameService : IUsernameService
    {
        private readonly IMembershipService membershipService;

        public UsernameService(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        public string Calculate(string currentValue)
        {
            int uniqueValue = 0;

            string newValue = currentValue;

            while (this.membershipService.GetUser(newValue) != null)
            {
                newValue = currentValue + uniqueValue;
                uniqueValue++;
            }

            return newValue;
        }
    }
}