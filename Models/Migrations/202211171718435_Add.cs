namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OPeriodLessons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDOrganization = c.String(maxLength: 128),
                        PeriodStartTime = c.Int(nullable: false),
                        PeriodEndTime = c.Int(nullable: false),
                        PeriodName = c.String(),
                        IDShift = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .ForeignKey("dbo.OShift", t => t.IDShift, cascadeDelete: true)
                .Index(t => t.IDOrganization)
                .Index(t => t.IDShift);
            
            AlterColumn("dbo.OShift", "ShiftStartTime", c => c.Int(nullable: false));
            AlterColumn("dbo.OShift", "ShiftEndTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OPeriodLessons", "IDShift", "dbo.OShift");
            DropForeignKey("dbo.OPeriodLessons", "IDOrganization", "dbo.Organization");
            DropIndex("dbo.OPeriodLessons", new[] { "IDShift" });
            DropIndex("dbo.OPeriodLessons", new[] { "IDOrganization" });
            AlterColumn("dbo.OShift", "ShiftEndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OShift", "ShiftStartTime", c => c.DateTime(nullable: false));
            DropTable("dbo.OPeriodLessons");
        }
    }
}
