using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace SkautSIS.Core {
    public class Migrations : DataMigrationImpl {

        public int Create() 
        {
            SchemaBuilder.CreateTable("GeneralSettingsPartRecord", 
                table => table
                .ContentPartRecord()
                .Column<int>("UnitId")
                .Column("AppId", DbType.Guid)
                .Column<string>("SkautIsServicesUrl")
                .Column<string>("SkautSisServicesUrl")
            );

            ContentDefinitionManager.AlterTypeDefinition("SkautSisSettings",
                cfg => cfg
                    .WithPart("GeneralSettingsPart")
            );

            return 1;
        }
    }
}