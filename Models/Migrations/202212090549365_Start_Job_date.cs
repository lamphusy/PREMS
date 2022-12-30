namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Start_Job_date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teacher", "StartJobDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Teacher", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teacher", "CreateDate", c => c.String());
            DropColumn("dbo.Teacher", "StartJobDate");
        }
    }
}
