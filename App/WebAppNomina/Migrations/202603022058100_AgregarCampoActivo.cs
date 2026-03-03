namespace WebAppNomina.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarCampoActivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empleadoes", "activo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empleadoes", "activo");
        }
    }
}
