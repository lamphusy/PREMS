namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PeriodName_OPeriodLesson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OPeriodLessons", "PeriodName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OPeriodLessons", "PeriodName");
        }
    }
}
