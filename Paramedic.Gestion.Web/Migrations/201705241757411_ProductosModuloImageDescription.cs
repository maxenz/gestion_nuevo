namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductosModuloImageDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductosModulos", "PathImagenAddon", c => c.String());
            AddColumn("dbo.ProductosModulos", "DescripcionAddon", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductosModulos", "DescripcionAddon");
            DropColumn("dbo.ProductosModulos", "PathImagenAddon");
        }
    }
}
