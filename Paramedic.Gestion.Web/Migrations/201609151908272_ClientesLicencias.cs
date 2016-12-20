namespace Gestion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientesLicencias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientesLicencias", "Alias", c => c.String());
            AddColumn("dbo.ClientesLicencias", "AndroidPassword", c => c.String());
            AddColumn("dbo.ClientesLicencias", "AndroidUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientesLicencias", "AndroidUrl");
            DropColumn("dbo.ClientesLicencias", "AndroidPassword");
            DropColumn("dbo.ClientesLicencias", "Alias");
        }
    }
}
