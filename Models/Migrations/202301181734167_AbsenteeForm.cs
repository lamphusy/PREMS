namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbsenteeForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbsenteeForm", "IDShift", c => c.Int(nullable: false));
            AlterColumn("dbo.AbsenteeForm", "Status", c => c.Int(nullable: false));
            CreateIndex("dbo.AbsenteeForm", "IDShift");
            AddForeignKey("dbo.AbsenteeForm", "IDShift", "dbo.OShift", "ID", cascadeDelete: true);
            DropColumn("dbo.AbsenteeForm", "Shift");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AbsenteeForm", "Shift", c => c.Int(nullable: false));
            DropForeignKey("dbo.AbsenteeForm", "IDShift", "dbo.OShift");
            DropIndex("dbo.AbsenteeForm", new[] { "IDShift" });
            AlterColumn("dbo.AbsenteeForm", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.AbsenteeForm", "IDShift");
        }
    }
}
