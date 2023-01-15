namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_IndexOfClass : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Studies", "IndexInClass");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Studies", "IndexInClass", c => c.Int(nullable: false));
        }
    }
}
