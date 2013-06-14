using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace SkautSIS.Synchronization
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable("SyncSettingsPartRecord",
                table => table
                .ContentPartRecord()
                .Column<bool>("AllowAutomatedSynchronization")
                .Column<bool>("SyncFrequencyType")
                .Column<bool>("SyncFrequency")
            );

            ContentDefinitionManager.AlterTypeDefinition("SyncSettings",
                cfg => cfg
                    .WithPart("SyncSettingsPart")
            );

            return 1;
        }
    }
}