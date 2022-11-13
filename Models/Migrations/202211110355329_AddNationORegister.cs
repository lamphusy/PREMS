namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNationORegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ORegister", "Nation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ORegister", "Nation");
        }
    }
}
