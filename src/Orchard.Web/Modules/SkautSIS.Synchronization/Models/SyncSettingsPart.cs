using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace SkautSIS.Synchronization.Models
{
    public class SyncSettingsPart : ContentPart<SyncSettingsPartRecord>, ISyncSettings
    {
        public bool AllowAutomatedSynchronization 
        { 
            get { return Record.AllowAutomatedSynchronization; } 
            set { Record.AllowAutomatedSynchronization = value; } 
        }

        public int SyncFrequencyType
        {
            get { return Record.SyncFrequencyType; }
            set { Record.SyncFrequencyType = value; }
        }

        public int SyncFrequency
        {
            get { return Record.SyncFrequency; }
            set { Record.SyncFrequency = value; }
        }

        public IEnumerable<dynamic> AvailableFrequencyType { get; set; } 
    }

    public enum Frequency
    {
        Days,
        Weeks,
        Months
    }
}