namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Class_organization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Class", "IDOrganization", c => c.String(maxLength: 128));
            CreateIndex("dbo.Class", "IDOrganization");
            AddForeignKey("dbo.Class", "IDOrganization", "dbo.Organization", "IdOrganization");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Class", "IDOrganization", "dbo.Organization");
            DropIndex("dbo.Class", new[] { "IDOrganization" });
            DropColumn("dbo.Class", "IDOrganization");
        }
    }
}
