using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.Services;

namespace SkautSIS.PersonGroups.ViewModels
{
    public class PersonGroupsIndexViewModel
    {
        public IList<PersonGroupEntry> PersonGroups { get; set; }
        public PersonGroupIndexOptions Options { get; set; }
        public dynamic Pager { get; set; }
    }

    public class PersonGroupEntry
    {
        public PersonGroupModel PersonGroup { get; set; }
        public bool IsChecked { get; set; }
    }

    public class PersonGroupIndexOptions
    {
        public string Search { get; set; }
        public PersonGroupsOrder Order { get; set; }
        public PersonGroupsFilter Filter { get; set; }
        public PersonGroupsBulkAction BulkAction { get; set; }
    }

    public enum PersonGroupsOrder
    {
        Title,
        Type
    }

    public enum PersonGroupsFilter
    {
        All,
        Custom,
        Unit,
        Event
    }

    public enum PersonGroupsBulkAction
    {
        None,
        Delete
    }
}