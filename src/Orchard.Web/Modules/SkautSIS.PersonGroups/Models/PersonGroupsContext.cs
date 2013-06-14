
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SkautSIS.PersonGroups.Models;

namespace SkautSIS.PersonGroups.PersonGroupsModel
{
    public partial class PersonGroupsContext
    {
        public IQueryable<Person> GetPersonGroupMembers(int personGroupId) {
            var query = CreateQuery<Person>("GetPersonGroupMembers");

            query = query.AddQueryOption("personGroupId", personGroupId);

            return query;
        }

        public void UpdatePersonGroupMembers(int personGroupId, IEnumerable<string> personIds) 
        {
            var stringIds = string.Empty;
            if (personIds != null && personIds.Count() > 0) {
                stringIds = string.Join(",", personIds);
            }

            var uri = new Uri(string.Format("UpdatePersonGroupMembers?personGroupId={0}&personIds='{1}'", personGroupId.ToString(), stringIds), UriKind.Relative);

            Execute<string>(uri);
        }
    }

    [IgnoreProperties("Type")]
    public partial class PersonGroup
    {
        public TypeEnum GetType()
        {
            return (TypeEnum)this.Type;
        }
        public void SetType(TypeEnum type)
        {
            this.Type = (int)type;
        }
    }
}