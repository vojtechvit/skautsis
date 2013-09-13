using Orchard;

namespace SkautSIS.Users.Services
{
    public interface IUsernameService : IDependency
    {
        string Calculate(string currentValue);
    }
}