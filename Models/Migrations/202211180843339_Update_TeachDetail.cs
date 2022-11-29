namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_TeachDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeachDetails", "IDShift", "dbo.OShift");
            DropIndex("dbo.TeachDetails", new[] { "IDShift" });
            AddColumn("dbo.TeachDetails", "IDPeriod", c => c.Int(nullable: false));
            CreateIndex("dbo.TeachDetails", "IDPeriod");
            AddForeignKey("dbo.TeachDetails", "IDPeriod", "dbo.OPeriodLessons", "ID", cascadeDelete: true);
            DropColumn("dbo.TeachDetails", "IDShift");
            DropColumn("dbo.TeachDetails", "Period");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TeachDetails", "Period", c => c.Int(nullable: false));
            AddColumn("dbo.TeachDetails", "IDShift", c => c.Int(nullable: false));
            DropForeignKey("dbo.TeachDetails", "IDPeriod", "dbo.OPeriodLessons");
            DropIndex("dbo.TeachDetails", new[] { "IDPeriod" });
            DropColumn("dbo.TeachDetails", "IDPeriod");
            CreateIndex("dbo.TeachDetails", "IDShift");
            AddForeignKey("dbo.TeachDetails", "IDShift", "dbo.OShift", "ID", cascadeDelete: true);
        }
    }
}
