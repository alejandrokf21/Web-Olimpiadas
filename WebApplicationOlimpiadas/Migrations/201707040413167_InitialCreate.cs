namespace WebApplicationOlimpiadas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TablaPruebas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Correlative = c.Int(nullable: false),
                        goodAnswer = c.Int(nullable: false),
                        wrongAnswer = c.Int(nullable: false),
                        blankAnswer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TablaPruebas");
        }
    }
}
