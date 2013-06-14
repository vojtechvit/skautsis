using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Data;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.ViewModels;

namespace SkautSIS.PersonGroups.Services
{
    public class PersonGroupsVisibilityService : IPersonGroupsVisibilityService 
    {
        public readonly IRepository<ContentPersonGroupsRecord> _contentPersonGroupsRepository;

        public PersonGroupsVisibilityService(
            IRepository<ContentPersonGroupsRecord> contentPersonGroupsRepository) 
        {
            _contentPersonGroupsRepository = contentPersonGroupsRepository;
        }

        public void UpdatePersonGroupsForContentItem(ContentItem item, IEnumerable<PersonGroupEntry> personGroups) 
        {
            var record = item.As<PersonGroupsVisibilityPart>().Record;
            
            var oldPersonGroups = _contentPersonGroupsRepository.Fetch(
                r => r.PersonGroupsVisibilityPartRecord == record);

            var lookupNew = personGroups
                .Where(e => e.IsChecked)
                .Select(e => e.PersonGroup.PersonGroupId)
                .ToDictionary(r => r, r => false);

            // Delete the person groups that are no longer there
            // and mark the ones that should stay
            foreach (var contentPersonGroupsRecord in oldPersonGroups)
            {
                if (lookupNew.ContainsKey(
                    contentPersonGroupsRecord.PersonGroupId))
                {

                    lookupNew[contentPersonGroupsRecord.PersonGroupId]
                        = true;
                }
                else
                {
                    _contentPersonGroupsRepository.Delete(contentPersonGroupsRecord);
                }
            }
            // Add the new person groups
            foreach (var groupId in lookupNew.Where(kvp => !kvp.Value)
                                           .Select(kvp => kvp.Key))
            {
                _contentPersonGroupsRepository.Create(new ContentPersonGroupsRecord
                {
                    PersonGroupsVisibilityPartRecord = record,
                    PersonGroupId = groupId
                });
            }
        }
    }
}