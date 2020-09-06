using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Environment.Configuration;
using Orchard.Logging;
using SkautSIS.Core.Models;
using System;
using System.Data;

namespace SkautSIS.Core
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

        public int UpdateFrom2()
        {
            var site = orchardServices.ContentManager.Get(1).As<SkautSisCoreSettingsPart>();

            var coreSettingsTable = this.GetPrefixedTableName("SkautSIS_Core_SkautSisCoreSettingsPartRecord");

            this.ExecuteReader("SELECT * FROM " + coreSettingsTable,
                (reader, connection) =>
                {
                    site.AppId = reader["AppId"] == DBNull.Value ? (Guid?)null : (Guid)reader["AppId"];
                    site.UseTestingWebServices = (bool)reader["UseTestingWebServices"];
                });

            SchemaBuilder.DropTable("SkautSisCoreSettingsPartRecord");
            
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
    }
}