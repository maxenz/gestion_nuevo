namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClienteNullableInTareaGestion : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TareasGestiones", new[] { "ClienteId" });
            AlterColumn("dbo.TareasGestiones", "ClienteId", c => c.Int());
            CreateIndex("dbo.TareasGestiones", "ClienteId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TareasGestiones", new[] { "ClienteId" });
            AlterColumn("dbo.TareasGestiones", "ClienteId", c => c.Int(nullable: false));
            CreateIndex("dbo.TareasGestiones", "ClienteId");
        }
    }
}
