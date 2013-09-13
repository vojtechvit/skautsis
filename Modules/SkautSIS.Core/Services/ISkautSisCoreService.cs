using Orchard;

namespace SkautSIS.Core.Services
{
    public interface ISkautSisCoreService : IDependency
    {
        void RefreshUnitInfo();
    }
}