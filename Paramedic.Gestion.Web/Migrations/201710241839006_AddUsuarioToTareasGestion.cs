namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsuarioToTareasGestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TareasGestiones", "UsuarioId", c => c.Int(nullable: false));
            CreateIndex("dbo.TareasGestiones", "UsuarioId");
            AddForeignKey("dbo.TareasGestiones", "UsuarioId", "dbo.UserProfile", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TareasGestiones", "UsuarioId", "dbo.UserProfile");
            DropIndex("dbo.TareasGestiones", new[] { "UsuarioId" });
            DropColumn("dbo.TareasGestiones", "UsuarioId");
        }
    }
}
