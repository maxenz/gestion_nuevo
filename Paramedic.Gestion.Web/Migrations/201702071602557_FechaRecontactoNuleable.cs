namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaRecontactoNuleable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClientesGestiones", "FechaRecontacto", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClientesGestiones", "FechaRecontacto", c => c.DateTime(nullable: false));
        }
    }
}
