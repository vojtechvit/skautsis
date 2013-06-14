using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace SkautSIS.Synchronization.Models
{
    public class SyncSettingsPartRecord : ContentPartRecord
    {
        public virtual bool AllowAutomatedSynchronization { get; set; }
        public virtual int SyncFrequencyType { get; set; }
        public virtual int SyncFrequency { get; set; }
    }
}