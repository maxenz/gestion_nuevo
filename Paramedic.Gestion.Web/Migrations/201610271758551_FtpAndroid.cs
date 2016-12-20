namespace Gestion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FtpAndroid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientesLicencias", "FtpAndroidDir", c => c.String());
            AddColumn("dbo.ClientesLicencias", "FtpAndroidUser", c => c.String());
            AddColumn("dbo.ClientesLicencias", "FtpAndroidPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientesLicencias", "FtpAndroidPassword");
            DropColumn("dbo.ClientesLicencias", "FtpAndroidUser");
            DropColumn("dbo.ClientesLicencias", "FtpAndroidDir");
        }
    }
}
