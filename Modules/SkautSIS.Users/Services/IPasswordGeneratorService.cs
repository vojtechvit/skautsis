using Orchard;

namespace SkautSIS.Users.Services
{
    public interface IPasswordGeneratorService : IDependency
    {
        string Generate();
    }
}