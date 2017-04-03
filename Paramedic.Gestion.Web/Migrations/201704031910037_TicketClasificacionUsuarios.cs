namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketClasificacionUsuarios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketsClasificacionUsuarios",
                c => new
                    {
                        UserProfileId = c.Int(nullable: false),
                        TicketsClasificacionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserProfileId, t.TicketsClasificacionId })
                .ForeignKey("dbo.TicketsClasificaciones", t => t.TicketsClasificacionId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfileId)
                .Index(t => t.UserProfileId)
                .Index(t => t.TicketsClasificacionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketsClasificacionUsuarios", "UserProfileId", "dbo.UserProfile");
            DropForeignKey("dbo.TicketsClasificacionUsuarios", "TicketsClasificacionId", "dbo.TicketsClasificaciones");
            DropIndex("dbo.TicketsClasificacionUsuarios", new[] { "TicketsClasificacionId" });
            DropIndex("dbo.TicketsClasificacionUsuarios", new[] { "UserProfileId" });
            DropTable("dbo.TicketsClasificacionUsuarios");
        }
    }
}
