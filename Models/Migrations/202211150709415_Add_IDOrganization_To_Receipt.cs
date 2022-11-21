namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IDOrganization_To_Receipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receipt", "IDOrganization", c => c.String(maxLength: 128));
            CreateIndex("dbo.Receipt", "IDOrganization");
            AddForeignKey("dbo.Receipt", "IDOrganization", "dbo.Organization", "IdOrganization");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receipt", "IDOrganization", "dbo.Organization");
            DropIndex("dbo.Receipt", new[] { "IDOrganization" });
            DropColumn("dbo.Receipt", "IDOrganization");
        }
    }
}
