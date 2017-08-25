namespace WebApplicationOlimpiadas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TablaPruebas", "Course", c => c.Int(nullable: false));
            AddColumn("dbo.TablaPruebas", "Level", c => c.Int(nullable: false));
            AlterColumn("dbo.TablaPruebas", "Correlative", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TablaPruebas", "Correlative", c => c.Int(nullable: false));
            DropColumn("dbo.TablaPruebas", "Level");
            DropColumn("dbo.TablaPruebas", "Course");
        }
    }
}
