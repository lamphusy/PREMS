namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_semester : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Semesters", "IDYear", c => c.Int(nullable: false));
            CreateIndex("dbo.Semesters", "IDYear");
            AddForeignKey("dbo.Semesters", "IDYear", "dbo.SchoolYears", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Semesters", "IDYear", "dbo.SchoolYears");
            DropIndex("dbo.Semesters", new[] { "IDYear" });
            DropColumn("dbo.Semesters", "IDYear");
        }
    }
}
