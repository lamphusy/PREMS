namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStatistic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Statistics", "NumOfOrganization", c => c.Int(nullable: false));
            AddColumn("dbo.Statistics", "NumOfStudent", c => c.Int(nullable: false));
            AddColumn("dbo.Statistics", "NumOfTeacher", c => c.Int(nullable: false));
            AlterColumn("dbo.Organization", "CreateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Statistics", "NumOfOrganinze");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Statistics", "NumOfOrganinze", c => c.Int(nullable: false));
            AlterColumn("dbo.Organization", "CreateDate", c => c.DateTime());
            DropColumn("dbo.Statistics", "NumOfTeacher");
            DropColumn("dbo.Statistics", "NumOfStudent");
            DropColumn("dbo.Statistics", "NumOfOrganization");
        }
    }
}
