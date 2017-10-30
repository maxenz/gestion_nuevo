namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObservacionesNullableInTareaGestion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TareasGestiones", "Observaciones", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TareasGestiones", "Observaciones", c => c.String(nullable: false));
        }
    }
}
