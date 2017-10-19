namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClasificacionProyectos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClasificacionesProyectos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Proyectos", "ClasificacionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Proyectos", "ClasificacionId");
            AddForeignKey("dbo.Proyectos", "ClasificacionId", "dbo.ClasificacionesProyectos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Proyectos", "ClasificacionId", "dbo.ClasificacionesProyectos");
            DropIndex("dbo.Proyectos", new[] { "ClasificacionId" });
            DropColumn("dbo.Proyectos", "ClasificacionId");
            DropTable("dbo.ClasificacionesProyectos");
        }
    }
}
