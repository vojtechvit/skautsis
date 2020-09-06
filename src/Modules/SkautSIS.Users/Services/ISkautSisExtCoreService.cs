using Orchard;

namespace SkautSIS.Users.Services
{
    public interface ISkautSisExtCoreService : IDependency
    {
        void RefreshUnitInfo();
    }
}