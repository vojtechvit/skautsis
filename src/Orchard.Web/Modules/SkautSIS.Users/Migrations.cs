using System.Data;
using Orchard.Data.Migration;

namespace SkautSIS.Users 
{
    public class UsersDataMigration : DataMigrationImpl 
    {

        public int Create() 
        {
            SchemaBuilder.CreateTable("SkautIsUserPartRecord", 
                table => table
                    .ContentPartRecord()
                    .Column<int>("SkautIsUserId")
                    .Column<int>("SkautIsPersonId")
                    .Column<string>("UserName")
                    .Column<int>("SkautIsRoleId")
                    .Column<int>("SkautIsUnitId")
                    .Column("SkautIsToken", DbType.Guid)
                    .Column("SkautIsTokenExpiration", DbType.DateTime)
                );

            SchemaBuilder.CreateTable("RegistrationSettingsPartRecord", 
                table => table
                    .ContentPartRecord()
                    .Column<bool>("UsersCanRegister", c => c.WithDefault(false))
                    .Column<bool>("EnableLostPassword", c => c.WithDefault(false))
                );

            return 1;
        }
    }
}