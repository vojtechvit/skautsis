using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.Handlers
{
    public class SkautIsUserPartHandler : ContentHandler
    {
        public SkautIsUserPartHandler(
            IRepository<SkautIsUserPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<SkautIsUserPart>("User"));
        }
    }
}