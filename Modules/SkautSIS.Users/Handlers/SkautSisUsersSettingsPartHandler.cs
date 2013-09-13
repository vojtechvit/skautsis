using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using SkautSIS.Users.Models;

namespace SkautSIS.Users.Handlers
{
    public class SkautSisUsersSettingsPartHandler : ContentHandler
    {
        public SkautSisUsersSettingsPartHandler(IRepository<SkautSisUsersSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;
            Filters.Add(StorageFilter.For(repository));
            Filters.Add(new ActivatingFilter<SkautSisUsersSettingsPart>("Site"));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;

            base.GetItemMetadata(context);

            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("SkautSIS Users")));
        }
    }
}