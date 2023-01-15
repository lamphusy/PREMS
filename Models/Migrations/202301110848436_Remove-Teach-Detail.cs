namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTeachDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeachDetails", "IDPeriod", "dbo.OPeriodLessons");
            DropForeignKey("dbo.TeachDetails", "IDTeach", "dbo.Teach");
            DropIndex("dbo.TeachDetails", new[] { "IDTeach" });
            DropIndex("dbo.TeachDetails", new[] { "IDPeriod" });
            DropPrimaryKey("dbo.Teach");
            DropColumn("dbo.Teach", "IDTeach");
            AddColumn("dbo.Teach", "ID", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Teach", "IDSchoolYear", c => c.Int(nullable: false));
            AddColumn("dbo.Teach", "IDPeriod", c => c.Int(nullable: false));
            AddColumn("dbo.Teach", "WeekDay", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Teach", "ID");
            CreateIndex("dbo.Teach", "IDSchoolYear");
            CreateIndex("dbo.Teach", "IDPeriod");
            AddForeignKey("dbo.Teach", "IDPeriod", "dbo.OPeriodLessons", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Teach", "IDSchoolYear", "dbo.SchoolYears", "ID", cascadeDelete: true);
            DropTable("dbo.TeachDetails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TeachDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDTeach = c.Int(nullable: false),
                        IDPeriod = c.Int(nullable: false),
                        WeekDay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Teach", "IDTeach", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Teach", "IDSchoolYear", "dbo.SchoolYears");
            DropForeignKey("dbo.Teach", "IDPeriod", "dbo.OPeriodLessons");
            DropIndex("dbo.Teach", new[] { "IDPeriod" });
            DropIndex("dbo.Teach", new[] { "IDSchoolYear" });
            DropPrimaryKey("dbo.Teach");
            DropColumn("dbo.Teach", "WeekDay");
            DropColumn("dbo.Teach", "IDPeriod");
            DropColumn("dbo.Teach", "IDSchoolYear");
            DropColumn("dbo.Teach", "ID");
            AddPrimaryKey("dbo.Teach", "IDTeach");
            CreateIndex("dbo.TeachDetails", "IDPeriod");
            CreateIndex("dbo.TeachDetails", "IDTeach");
            AddForeignKey("dbo.TeachDetails", "IDTeach", "dbo.Teach", "IDTeach", cascadeDelete: true);
            AddForeignKey("dbo.TeachDetails", "IDPeriod", "dbo.OPeriodLessons", "ID", cascadeDelete: true);
        }
    }
}
