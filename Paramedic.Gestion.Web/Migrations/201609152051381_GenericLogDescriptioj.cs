namespace Gestion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenericLogDescriptioj : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LicenciasLogs", "GenericDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LicenciasLogs", "GenericDescription");
        }
    }
}
