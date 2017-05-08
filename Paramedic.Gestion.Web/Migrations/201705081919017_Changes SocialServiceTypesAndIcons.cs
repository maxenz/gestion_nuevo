namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesSocialServiceTypesAndIcons : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SocialServiceTypes", "Configuration", c => c.String(nullable: false));
            AddColumn("dbo.SocialServiceTypes", "SocialMediaType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SocialServiceTypes", "SocialMediaType");
            DropColumn("dbo.SocialServiceTypes", "Configuration");
        }
    }
}
