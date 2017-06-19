namespace Paramedic.Gestion.Web
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTituloToNoticia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Noticias", "Titulo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Noticias", "Titulo");
        }
    }
}
