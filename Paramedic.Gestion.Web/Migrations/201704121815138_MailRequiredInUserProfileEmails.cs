namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MailRequiredInUserProfileEmails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfileEmails", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfileEmails", "Email", c => c.String());
        }
    }
}
