using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace SkautSIS.Core
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("SkautSisCoreSettingsPartRecord",
                table => table
                .ContentPartRecord()
                .Column("AppId", DbType.Guid)
                .Column<bool>("UseTestingWebServices")
                .Column<string>("UnitRegistrationNumber", c => c.WithLength(20))
                .Column<string>("UnitDisplayName", c => c.WithLength(200))
                .Column<int>("UnitId")
            );

            return 1;
        }
    }
}