namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NEWNEW : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Student", "IDStudent");
            AddForeignKey("dbo.Student", "IDStudent", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Student", "IDStudent", "dbo.ApplicationUsers");
            DropIndex("dbo.Student", new[] { "IDStudent" });
        }
    }
}
