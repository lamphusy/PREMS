namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateData : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Student", "FullName");
            DropColumn("dbo.Student", "Address");
            DropColumn("dbo.Student", "DateOfBirth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Student", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Student", "Address", c => c.String());
            AddColumn("dbo.Student", "FullName", c => c.String(nullable: false));
        }
    }
}
