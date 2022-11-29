namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_StartEnd_OShift : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OShift", "ShiftStartTime");
            DropColumn("dbo.OShift", "ShiftEndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OShift", "ShiftEndTime", c => c.Int(nullable: false));
            AddColumn("dbo.OShift", "ShiftStartTime", c => c.Int(nullable: false));
        }
    }
}
