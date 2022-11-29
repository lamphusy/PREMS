namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_IdOrganization_Semester : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Semesters", "IDOrganization", "dbo.Organization");
            DropIndex("dbo.Semesters", new[] { "IDOrganization" });
            DropColumn("dbo.Semesters", "IDOrganization");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Semesters", "IDOrganization", c => c.String(maxLength: 128));
            CreateIndex("dbo.Semesters", "IDOrganization");
            AddForeignKey("dbo.Semesters", "IDOrganization", "dbo.Organization", "IdOrganization");
        }
    }
}
