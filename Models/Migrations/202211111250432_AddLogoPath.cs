namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogoPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organization", "LogoPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organization", "LogoPath");
        }
    }
}
