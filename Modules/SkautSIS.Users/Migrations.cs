using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Data.Migration;

namespace SkautSIS.Users
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("SkautSisCoreExtSettingsPartRecord",
                table => table
                .ContentPartRecord()
                .Column<string>("UnitTypeId", c => c.WithLength(100))
            );

            SchemaBuilder.CreateTable("SkautSisUsersSettingsPartRecord",
                table => table
                .ContentPartRecord()
                .Column<string>("RolesAssignedByMembership", c => c.WithLength(500))
                .Column<string>("RolesAssignedBySkautIsRoles", c => c.WithLength(500))
            );

            SchemaBuilder.CreateTable("SkautIsUserPartRecord",
                table => table
                .ContentPartRecord()
                .Column<int>("SkautIsUserId")
                .Column<string>("SkautIsUserName", c => c.WithLength(200))
                .Column<int>("PersonId")
                .Column("Token", DbType.Guid)
                .Column<int>("RoleId")
                .Column<DateTime>("TokenExpiration")
                .Column<bool>("UnitId")
                .Column<bool>("HasMembership")
                .Column<string>("SkautIsRoles", c => c.WithLength(300))
                .Column<string>("FirstName", c => c.WithLength(100))
                .Column<string>("LastName", c => c.WithLength(100))
                .Column<string>("NickName", c => c.WithLength(100))
                .Column<DateTime>("BirthDate")
            );

            SchemaBuilder.AlterTable("SkautIsUserPartRecord",
                table => table.CreateIndex("IX_SkautIsUserId", "SkautIsUserId"));

            return 1;
        }
    }
}