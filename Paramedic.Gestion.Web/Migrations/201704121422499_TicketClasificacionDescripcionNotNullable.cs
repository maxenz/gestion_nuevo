namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketClasificacionDescripcionNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TicketsClasificaciones", "Descripcion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketsClasificaciones", "Descripcion", c => c.String());
        }
    }
}
