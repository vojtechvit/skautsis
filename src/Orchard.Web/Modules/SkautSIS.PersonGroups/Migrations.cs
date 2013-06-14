using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data;
using Orchard.Data.Migration;
using SkautSIS.PersonGroups.Models;
using SkautSIS.PersonGroups.Services;

namespace SkautSIS.PersonGroups 
{
    public class Migrations : DataMigrationImpl {
 
        public int Create() 
        {
            SchemaBuilder.CreateTable("PersonGroupsVisibilityPartRecord", 
                table => table
                .ContentPartRecord()
            );

            SchemaBuilder.CreateTable("ContentPersonGroupsRecord",
                table => table
                .Column<int>("Id", column => column.PrimaryKey().Identity())
                .Column<int>("PersonGroupsVisibilityPartRecord_Id")
                .Column<int>("PersonGroupId")
            );

            ContentDefinitionManager.AlterPartDefinition("PersonGroupsVisibilityPart",
                builder => builder.Attachable()
            );

            return 1;
        }
    }
}