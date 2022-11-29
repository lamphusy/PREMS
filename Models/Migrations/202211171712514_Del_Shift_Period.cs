namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Del_Shift_Period : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OPeriodLessons", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.OPeriodLessons", "IDShift", "dbo.OShift");
            DropIndex("dbo.OPeriodLessons", new[] { "IDOrganization" });
            DropIndex("dbo.OPeriodLessons", new[] { "IDShift" });
            DropTable("dbo.OPeriodLessons");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OPeriodLessons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDOrganization = c.String(maxLength: 128),
                        PeriodStartTime = c.DateTime(nullable: false),
                        PeriodEndTime = c.DateTime(nullable: false),
                        PeriodName = c.String(),
                        IDShift = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.OPeriodLessons", "IDShift");
            CreateIndex("dbo.OPeriodLessons", "IDOrganization");
            AddForeignKey("dbo.OPeriodLessons", "IDShift", "dbo.OShift", "ID", cascadeDelete: true);
            AddForeignKey("dbo.OPeriodLessons", "IDOrganization", "dbo.Organization", "IdOrganization");
        }
    }
}
