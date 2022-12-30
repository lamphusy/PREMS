namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_property_Teachers : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Teacher", "DateOfBirth");
            DropColumn("dbo.Teacher", "Address");
            DropColumn("dbo.Teacher", "Gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teacher", "Gender", c => c.String());
            AddColumn("dbo.Teacher", "Address", c => c.String());
            AddColumn("dbo.Teacher", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
