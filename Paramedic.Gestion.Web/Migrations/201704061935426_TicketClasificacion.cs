namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketClasificacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "TicketsClasificacionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "TicketsClasificacionId");
            AddForeignKey("dbo.Tickets", "TicketsClasificacionId", "dbo.TicketsClasificaciones", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TicketsClasificacionId", "dbo.TicketsClasificaciones");
            DropIndex("dbo.Tickets", new[] { "TicketsClasificacionId" });
            DropColumn("dbo.Tickets", "TicketsClasificacionId");
        }
    }
}
