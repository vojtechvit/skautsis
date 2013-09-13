using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.Handlers
{
    public class SkautSisCoreExtSettingsPartHandler : ContentHandler
    {
        public SkautSisCoreExtSettingsPartHandler(IRepository<SkautSisCoreExtSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<SkautSisCoreExtSettingsPart>("Site"));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;

            base.GetItemMetadata(context);

            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("SkautSIS")));
        }
    }
}