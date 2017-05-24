namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntentosRealizados : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientesLicenciasProductosModulos", "IntentosRealizados", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientesLicenciasProductosModulos", "IntentosRealizados");
        }
    }
}
