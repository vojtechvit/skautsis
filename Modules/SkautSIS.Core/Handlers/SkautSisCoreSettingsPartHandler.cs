using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using SkautSIS.Core.Models;

namespace SkautSIS.Core.Handlers
{
    public class SkautSisCoreSettingsPartHandler : ContentHandler
    {
        public SkautSisCoreSettingsPartHandler(IRepository<SkautSisCoreSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<SkautSisCoreSettingsPart>("Site"));
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