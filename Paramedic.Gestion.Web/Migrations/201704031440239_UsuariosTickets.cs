namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuariosTickets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfileEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserProfileId = c.Int(nullable: false),
                        Email = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId)
                .Index(t => t.UserProfileId);
            
            AddColumn("dbo.UserProfile", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserProfile", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.UserProfile", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserProfile", "UpdatedBy", c => c.String(maxLength: 256));
            DropColumn("dbo.UserProfile", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "Email", c => c.String());
            DropForeignKey("dbo.UserProfileEmails", "UserProfileId", "dbo.UserProfile");
            DropIndex("dbo.UserProfileEmails", new[] { "UserProfileId" });
            DropColumn("dbo.UserProfile", "UpdatedBy");
            DropColumn("dbo.UserProfile", "UpdatedDate");
            DropColumn("dbo.UserProfile", "CreatedBy");
            DropColumn("dbo.UserProfile", "CreatedDate");
            DropTable("dbo.UserProfileEmails");
        }
    }
}
