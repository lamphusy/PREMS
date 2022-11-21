namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BankCode_Receipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receipt", "BankCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Receipt", "BankCode");
        }
    }
}
