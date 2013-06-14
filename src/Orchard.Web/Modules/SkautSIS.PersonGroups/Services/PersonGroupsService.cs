using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Orchard;
using Orchard.Security;
using SkautSIS.Core.Services;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.PersonGroupsModel;
using SkautSIS.Users.Models;
using SkautSIS.Users.Services;

namespace SkautSIS.PersonGroups.Services
{
    public class PersonGroupsService : IPersonGroupsService 
    {
        private readonly Lazy<PersonGroupsContext> _personGroupsContext;
        private readonly IUserCentralizationService _userService;

        public PersonGroupsService(
            ISkautSisServices skautSisServices,
            IUserCentralizationService userService) 
        {
            _userService = userService;
            _personGroupsContext = new Lazy<PersonGroupsContext>(
                () => {
                    var serviceUrl = skautSisServices.GetServiceUrl("PersonGroupsService");
                    return new PersonGroupsContext(new Uri(serviceUrl));
                });
        }

        public IPersonGroup GetSingle(int id)
        {
            var group = _personGroupsContext.Value.PersonGroups.Where(p => p.PersonGroupId == id).FirstOrDefault();

            var members = new Collection<int>();
            foreach (var person in _personGroupsContext.Value.GetPersonGroupMembers(group.PersonGroupId))
            {
                members.Add(person.SkautIsUserId);
            }

            var newGroup = new PersonGroupModel
            {
                PersonGroupId = group.PersonGroupId,
                DisplayName = group.DisplayName,
                Type = (TypeEnum)group.Type,
                MemberSkautIsIds = members
            };

            return newGroup;
        }

        public IPersonGroup GetSingle(string name) 
        {
            var group = _personGroupsContext.Value.PersonGroups.Where(g => g.DisplayName.Equals(name)).FirstOrDefault();

            var members = new Collection<int>();
            foreach (var person in _personGroupsContext.Value.GetPersonGroupMembers(group.PersonGroupId))
            {
                members.Add(person.SkautIsUserId);
            }

            return new PersonGroupModel
            {
                PersonGroupId = group.PersonGroupId,
                DisplayName = group.DisplayName,
                Type = (TypeEnum)group.Type,
                MemberSkautIsIds = members
            };
        }

        public IEnumerable<string> GetMembers(int id) 
        {
            throw new NotImplementedException();
        }

        public void UpdateMembers(int id, IEnumerable<string> personIds) 
        {
            _personGroupsContext.Value.UpdatePersonGroupMembers(id, personIds);
        }

        public IEnumerable<IPersonGroup> GetAll() 
        {
            var model = _personGroupsContext.Value.PersonGroups.ToList();

            var personGroups = new Collection<IPersonGroup>();
            foreach (var group in model) {
                personGroups.Add(new PersonGroupModel {
                    PersonGroupId = group.PersonGroupId,
                    DisplayName = group.DisplayName,
                    Type = (TypeEnum)group.Type
                    });
            }

            return personGroups;
        }

        public void Create(IPersonGroup personGroup) 
        {
            var newPersonGroup = new PersonGroup 
            {
                DisplayName = personGroup.DisplayName,
                Type = (int)personGroup.Type
            };

            _personGroupsContext.Value.AddToPersonGroups(newPersonGroup);
            _personGroupsContext.Value.SaveChanges();

            var group = _personGroupsContext.Value.PersonGroups.Where(g => g.DisplayName.Equals(personGroup.DisplayName)).FirstOrDefault();
            _personGroupsContext.Value.UpdatePersonGroupMembers(group.PersonGroupId, personGroup.MemberSkautIsIds.Select(x => x.ToString()));
        }

        public void CreateOrUpdate(IPersonGroup personGroup) 
        {
            if (_personGroupsContext.Value.PersonGroups.Where(g => g.DisplayName.Equals(personGroup.DisplayName)).FirstOrDefault() == null)
            {
                this.Create(personGroup);
                return;
            }

            this.Update(personGroup);
        }

        public void Update(IPersonGroup personGroup) 
        {
            var group = _personGroupsContext.Value.PersonGroups.Where(g => g.PersonGroupId == personGroup.PersonGroupId).FirstOrDefault();
            if (group != null) 
            {
                if(!group.DisplayName.Equals(personGroup.DisplayName)) 
                {
                    group.DisplayName = personGroup.DisplayName;
                    _personGroupsContext.Value.UpdateObject(group);
                    _personGroupsContext.Value.SaveChanges();
                }
                _personGroupsContext.Value.UpdatePersonGroupMembers(group.PersonGroupId, personGroup.MemberSkautIsIds.Select(x => x.ToString()));
            }
        }

        public void Delete(int id) {
            var group = _personGroupsContext.Value.PersonGroups.Where(g => g.PersonGroupId == id).FirstOrDefault();
            if (group != null) {
                _personGroupsContext.Value.DeleteObject(group);
                _personGroupsContext.Value.SaveChanges();
            }
        }
        
        public void CreatePerson(int skautIsUserId) {
            if (_personGroupsContext.Value.Persons.
                Where(p => p.SkautIsUserId == skautIsUserId).FirstOrDefault() != null) return;

            _personGroupsContext.Value.AddToPersons(new Person { SkautIsUserId = skautIsUserId });
            _personGroupsContext.Value.SaveChanges();
        }

        public bool VerifyUnicity(string name) 
        {
            return this.VerifyUnicity(name, null);
            //return (_personGroupsContext.PersonGroups.Where(g => g.DisplayName.Equals(name)).FirstOrDefault() == null) ? true : false;
        }

        public bool VerifyUnicity(string name, int? id) 
        {
            var personGroup = _personGroupsContext.Value.PersonGroups.Where(g => g.DisplayName.Equals(name)).FirstOrDefault();
            if (personGroup == null) return true;

            return (id.HasValue && personGroup.PersonGroupId == id.Value);
        }
    }
}