namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailPrincipal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileEmails", "EmailPrincipal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfileEmails", "EmailPrincipal");
        }
    }
}
