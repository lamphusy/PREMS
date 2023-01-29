namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSomething : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScoreDetail", "IDScoreType", c => c.Int(nullable: false));
            CreateIndex("dbo.ScoreDetail", "IDScoreType");
            AddForeignKey("dbo.ScoreDetail", "IDScoreType", "dbo.ScoreType", "IDScoreType", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScoreDetail", "IDScoreType", "dbo.ScoreType");
            DropIndex("dbo.ScoreDetail", new[] { "IDScoreType" });
            AlterColumn("dbo.ScoreDetail", "IDScoreType", c => c.String(nullable: false));
        }
    }
}
