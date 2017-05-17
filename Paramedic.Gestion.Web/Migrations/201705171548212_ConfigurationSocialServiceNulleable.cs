namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigurationSocialServiceNulleable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SocialServices", "Configuration", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SocialServices", "Configuration", c => c.String(nullable: false));
        }
    }
}
