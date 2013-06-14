using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using SkautSIS.Services.PersonGroups.Models;

namespace SkautSIS.Services.PersonGroups.Services
{
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class PersonGroupsService : DataService<PersonGroupsContext>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config) {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.UseVerboseErrors = true;
        }

        [WebGet]
        public IQueryable<Person> GetPersonGroupMembers(int personGroupId)
        {
            var query = CurrentDataSource.Persons as IQueryable<Person>;

            return query.Where(p => p.Groups.Any(g => g.PersonGroupId == personGroupId));
        }

        [WebGet]
        public void UpdatePersonGroupMembers(int personGroupId, string personIds)
        {
            var personGroup = CurrentDataSource.PersonGroups.Where(g => g.PersonGroupId == personGroupId).FirstOrDefault();
            if (personGroup == null) return;
            var oldPersonIds = personGroup.Persons.Select(p => p.SkautIsUserId).ToList();

            var lookupNew = new Dictionary<int, bool>();
            var stringIds = string.IsNullOrWhiteSpace(personIds) ? null : personIds.Split(',');
            if (stringIds != null && stringIds.Count() > 0)
            {
                var newPersonIds = stringIds.Select(x => int.Parse(x));

                lookupNew = newPersonIds.ToDictionary(r => r, r => false);
            }
            // Delete the persons that are no longer there
            // and mark the ones that should stay
            foreach (var personId in oldPersonIds)
            {
                if (lookupNew.ContainsKey(personId))
                {
                    lookupNew[personId] = true;
                }
                else
                {
                    var p = personGroup.Persons.Single(x => x.SkautIsUserId == personId);
                    personGroup.Persons.Remove(p);
                }
            }
            // Add the new persons
            foreach (var personId in lookupNew.Where(kvp => !kvp.Value).Select(kvp => kvp.Key))
            {
                var person = CurrentDataSource.Persons.Where(p => p.SkautIsUserId == personId).FirstOrDefault();
                if (person == null) throw new ArgumentException(string.Format("{0} is not a valid person Id. The system might need to be synchronized.", personId));
                personGroup.Persons.Add(person);
            }

            CurrentDataSource.SaveChanges();
        }
    }
}
