namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_TypeVarialable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Teacher", "Salary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Teacher", "CoefficientsSalary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teacher", "CoefficientsSalary", c => c.Double(nullable: false));
            AlterColumn("dbo.Teacher", "Salary", c => c.Double(nullable: false));
        }
    }
}
