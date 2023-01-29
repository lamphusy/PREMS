namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class typescoreidchangedatatype : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ScoreType");
            AlterColumn("dbo.ScoreType", "IDScoreType", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ScoreType", "IDScoreType");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ScoreType");
            AlterColumn("dbo.ScoreType", "IDScoreType", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ScoreType", "IDScoreType");
        }
    }
}
