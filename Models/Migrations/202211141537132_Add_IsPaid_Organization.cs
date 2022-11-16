namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsPaid_Organization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "IsPaid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "IsPaid");
        }
    }
}
