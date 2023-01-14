namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_DataType_TransferClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClassTransferringForm", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClassTransferringForm", "Status", c => c.Boolean(nullable: false));
        }
    }
}
