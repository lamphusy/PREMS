namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ReferencesLink_Organization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "FacebookLink", c => c.String());
            AddColumn("dbo.Organization", "InstagramLink", c => c.String());
            AddColumn("dbo.Organization", "LinkedinLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "LinkedinLink");
            DropColumn("dbo.Organization", "InstagramLink");
            DropColumn("dbo.Organization", "FacebookLink");
        }
    }
}
