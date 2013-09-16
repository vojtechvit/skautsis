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
            );

            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("SkautSisCoreSettingsPartRecord",
                table => table.DropColumn("UnitRegistrationNumber"));

            SchemaBuilder.AlterTable("SkautSisCoreSettingsPartRecord",
                table => table.DropColumn("UnitDisplayName"));

            SchemaBuilder.AlterTable("SkautSisCoreSettingsPartRecord",
                table => table.DropColumn("UnitId"));

            return 2;
        }
    }
}