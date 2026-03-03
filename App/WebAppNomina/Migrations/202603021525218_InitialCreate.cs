namespace WebAppNomina.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogAuditoriaSalarios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        usuario = c.String(),
                        fechaActualizacion = c.DateTime(nullable: false),
                        DetalleCambio = c.String(),
                        salario = c.Long(nullable: false),
                        emp_no = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Departamentoes",
                c => new
                    {
                        dept_no = c.Int(nullable: false, identity: true),
                        dept_name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.dept_no);
            
            CreateTable(
                "dbo.Empleadoes",
                c => new
                    {
                        emp_no = c.Int(nullable: false, identity: true),
                        ci = c.String(nullable: false, maxLength: 20),
                        birth_date = c.DateTime(nullable: false),
                        first_name = c.String(nullable: false, maxLength: 50),
                        last_name = c.String(nullable: false, maxLength: 50),
                        hire_date = c.DateTime(nullable: false),
                        correo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.emp_no);
            
            CreateTable(
                "dbo.Salarios",
                c => new
                    {
                        emp_no = c.Int(nullable: false),
                        from_date = c.DateTime(nullable: false),
                        salary = c.Long(nullable: false),
                        to_date = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.emp_no, t.from_date })
                .ForeignKey("dbo.Empleadoes", t => t.emp_no, cascadeDelete: true)
                .Index(t => t.emp_no);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        emp_no = c.Int(nullable: false, identity: true),
                        usuario = c.String(nullable: false),
                        clave = c.String(nullable: false),
                        rol = c.String(),
                    })
                .PrimaryKey(t => t.emp_no);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Salarios", "emp_no", "dbo.Empleadoes");
            DropIndex("dbo.Salarios", new[] { "emp_no" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Salarios");
            DropTable("dbo.Empleadoes");
            DropTable("dbo.Departamentoes");
            DropTable("dbo.LogAuditoriaSalarios");
        }
    }
}
