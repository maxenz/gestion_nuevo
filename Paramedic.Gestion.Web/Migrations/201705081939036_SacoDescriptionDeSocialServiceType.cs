namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SacoDescriptionDeSocialServiceType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SocialServiceTypes", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SocialServiceTypes", "Description", c => c.String(nullable: false));
        }
    }
}
