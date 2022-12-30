namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeacherUpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teacher", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teacher", "Gender");
        }
    }
}
