using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Security;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.PersonGroupsModel;
using SkautSIS.Users.Models;

namespace SkautSIS.PersonGroups.Services
{
    public interface IPersonGroupsService : IDependency
    {
        IEnumerable<IPersonGroup> GetAll();
        void Create(IPersonGroup personGroup);
        void CreateOrUpdate(IPersonGroup personGroup);
        void Update(IPersonGroup personGroup);

        IPersonGroup GetSingle(string key);
        IPersonGroup GetSingle(int id);
        void Delete(int id);

        IEnumerable<string> GetMembers(int id);
        void UpdateMembers(int id, IEnumerable<string> personIds);
        void CreatePerson(int skautIsUserId);

        bool VerifyUnicity(string name);
        bool VerifyUnicity(string name, int? id);
    }
}