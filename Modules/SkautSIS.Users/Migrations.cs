using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Environment.Configuration;
using Orchard.Logging;
using SkautSIS.Users.Models;
using System;
using System.Data;

namespace SkautSIS.Users
{
    public class Migrations : DataMigrationImpl
    {
        private readonly IOrchardServices orchardServices;

        private readonly ShellSettings shellSettings;

        private readonly ISessionFactoryHolder sessionFactoryHolder;

        public Migrations(
            IOrchardServices orchardServices,
            ShellSettings shellSettings,
            ISessionFactoryHolder sessionFactoryHolder)
        {
            if (orchardServices == null)
            {
                throw new ArgumentNullException("orchardServices");
            }

            if (shellSettings == null)
            {
                throw new ArgumentNullException("shellSettings");
            }

            if (sessionFactoryHolder == null)
            {
                throw new ArgumentNullException("sessionFactoryHolder");
            }

            this.orchardServices = orchardServices;
            this.shellSettings = shellSettings;
            this.sessionFactoryHolder = sessionFactoryHolder;
        }

        public ILogger Logger { get; set; }

        public int Create()
        {
            SchemaBuilder.CreateTable("SkautSisCoreExtSettingsPartRecord",
                table => table
                .ContentPartRecord()
                .Column<string>("UnitRegistrationNumber", c => c.WithLength(20))
                .Column<string>("UnitDisplayName", c => c.WithLength(200))
                .Column<int>("UnitId")
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
                .Column<DateTime>("TokenExpiration")
                .Column<int>("RoleId")
                .Column<int>("UnitId")
                .Column<bool>("HasMembership")
                .Column<string>("SkautIsRoles", c => c.WithLength(300))
                .Column<string>("FirstName", c => c.WithLength(100))
                .Column<string>("LastName", c => c.WithLength(100))
                .Column<string>("NickName", c => c.WithLength(100))
                .Column<DateTime>("BirthDate")
            );

            SchemaBuilder.AlterTable("SkautIsUserPartRecord",
                table => table.CreateIndex("IX_SkautIsUserId", "SkautIsUserId"));

            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("SkautSisCoreExtSettingsPartRecord",
                table => table.AddColumn<string>("UnitRegistrationNumber", c => c.WithLength(20)));

            SchemaBuilder.AlterTable("SkautSisCoreExtSettingsPartRecord",
                table => table.AddColumn<string>("UnitDisplayName", c => c.WithLength(200)));

            SchemaBuilder.AlterTable("SkautSisCoreExtSettingsPartRecord",
                table => table.AddColumn<int>("UnitId"));

            return 2;
        }

        public int UpdateFrom2()
        {
            var coreExtSettings = orchardServices.ContentManager.Get(1).As<SkautSisCoreExtSettingsPart>();
            var coreExtSettingsTable = this.GetPrefixedTableName("SkautSIS_Users_SkautSisCoreExtSettingsPartRecord");

            this.ExecuteReader("SELECT * FROM " + coreExtSettingsTable,
                (reader, connection) =>
                {
                    coreExtSettings.UnitId = reader["UnitId"] == DBNull.Value ? (int?)null : (int)reader["UnitId"];
                    coreExtSettings.UnitRegistrationNumber = ConvertToString(reader["UnitRegistrationNumber"]);
                    coreExtSettings.UnitDisplayName = ConvertToString(reader["UnitDisplayName"]);
                    coreExtSettings.UnitTypeId = ConvertToString(reader["UnitTypeId"]);
                });

            var usersSettings = orchardServices.ContentManager.Get(1).As<SkautSisUsersSettingsPart>();
            var usersSettingsTable = this.GetPrefixedTableName("SkautSIS_Users_SkautSisUsersSettingsPartRecord");

            this.ExecuteReader("SELECT * FROM " + usersSettingsTable,
                (reader, connection) =>
                {
                    usersSettings.RolesAssignedByMembership = ConvertToString(reader["RolesAssignedByMembership"]);
                    usersSettings.RolesAssignedBySkautIsRoles = ConvertToString(reader["RolesAssignedBySkautIsRoles"]);
                });

            SchemaBuilder.DropTable("SkautSisCoreExtSettingsPartRecord");
            SchemaBuilder.DropTable("SkautSisUsersSettingsPartRecord");

            return 3;
        }

        private string GetPrefixedTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(this.shellSettings.DataTablePrefix))
            {
                return tableName;
            }

            return this.shellSettings.DataTablePrefix + "_" + tableName;
        }

        private void ExecuteReader(string sqlStatement, Action<IDataReader, IDbConnection> action)
        {
            using (var session = this.sessionFactoryHolder.GetSessionFactory().OpenSession())
            {
                var command = session.Connection.CreateCommand();
                command.CommandText = string.Format(sqlStatement);

                var reader = command.ExecuteReader();

                while (reader != null && reader.Read())
                {
                    try
                    {
                        if (action != null)
                        {
                            action(reader, session.Connection);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, "Error while executing custom SQL Statement in Upgrade.");
                    }
                }

                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }

        private static string ConvertToString(object readerValue)
        {
            return readerValue == DBNull.Value ? null : (string)readerValue;
        }
    }
}