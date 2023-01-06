namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IndexInClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Studies", "IndexInClass", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Studies", "IndexInClass");
        }
    }
}
