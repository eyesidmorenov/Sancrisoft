namespace Sancrisoft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cargos",
                c => new
                    {
                        IdCargo = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdCargo);
            
            CreateTable(
                "dbo.Aplicantes",
                c => new
                    {
                        IdAplicante = c.Int(nullable: false, identity: true),
                        Nombres = c.String(nullable: false),
                        Apellidos = c.String(nullable: false),
                        IdCargo = c.Int(nullable: false),
                        UrlFoto = c.String(),
                    })
                .PrimaryKey(t => t.IdAplicante)
                .ForeignKey("dbo.Cargos", t => t.IdCargo, cascadeDelete: true)
                .Index(t => t.IdCargo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Aplicantes", "IdCargo", "dbo.Cargos");
            DropIndex("dbo.Aplicantes", new[] { "IdCargo" });
            DropTable("dbo.Aplicantes");
            DropTable("dbo.Cargos");
        }
    }
}
