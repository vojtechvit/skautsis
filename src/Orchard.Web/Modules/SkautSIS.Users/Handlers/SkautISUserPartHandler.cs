using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;

namespace SkautSIS.Users.Handlers
{
    public class SkautISUserPartHandler : ContentHandler
    {
        private readonly IUserCentralizationService _userCentralizationService;
        private readonly ISkautIsUserService _skautIsUserService;

        public SkautISUserPartHandler(
            IRepository<SkautIsUserPartRecord> repository,
            IUserCentralizationService userCentralizationService,
            ISkautIsUserService skautIsUserService)
        {
            Filters.Add(new ActivatingFilter<SkautIsUserPart>("User"));
            Filters.Add(StorageFilter.For(repository));

            _userCentralizationService = userCentralizationService;
            _skautIsUserService = skautIsUserService;
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            var part = context.ContentItem.As<SkautIsUserPart>();

            if (part != null)
            {
                context.Metadata.Identity.Add("User.UserName", part.UserName);
            }
        }


        protected override void Loaded(LoadContentContext context)
        {
            var part = context.ContentItem.As<SkautIsUserPart>();

            var user = _userCentralizationService.GetUserCentralizedData(part.Record.SkautIsUserId);
            if (user == null) return;

            part.FirstName = user.FirstName;
            part.LastName = user.LastName;
            part.NickName = user.NickName;
            part.Email = user.Email;
        }
    }
}