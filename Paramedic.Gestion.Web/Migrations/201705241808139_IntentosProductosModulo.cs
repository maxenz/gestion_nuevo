namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntentosProductosModulo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductosModulos", "Intentos", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductosModulos", "Intentos");
        }
    }
}
