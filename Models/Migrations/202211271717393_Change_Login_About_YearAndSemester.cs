namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Login_About_YearAndSemester : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teach", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Class", "IDSemester", "dbo.Semesters");
            DropIndex("dbo.Class", new[] { "IDSemester" });
            DropIndex("dbo.Teach", new[] { "IDSemester" });
            RenameColumn(table: "dbo.TotalScoreSubject", name: "IDSemester", newName: "IDYear");
            RenameIndex(table: "dbo.TotalScoreSubject", name: "IX_IDSemester", newName: "IX_IDYear");
            CreateTable(
                "dbo.SchoolYears",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastYear = c.Int(nullable: false),
                        NextYear = c.Int(nullable: false),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.TotalScoreInYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IDStudent = c.String(maxLength: 128),
                        IDYear = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SchoolYears", t => t.IDYear, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.IDStudent)
                .Index(t => t.IDStudent)
                .Index(t => t.IDYear);
            
            AddColumn("dbo.Class", "IDYear", c => c.Int(nullable: false));
            CreateIndex("dbo.Class", "IDYear");
            AddForeignKey("dbo.Class", "IDYear", "dbo.SchoolYears", "ID", cascadeDelete: true);
            DropColumn("dbo.Semesters", "LastYear");
            DropColumn("dbo.Semesters", "NextYear");
            DropColumn("dbo.Class", "IDSemester");
            DropColumn("dbo.Teach", "IDSemester");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teach", "IDSemester", c => c.Int(nullable: false));
            AddColumn("dbo.Class", "IDSemester", c => c.Int(nullable: false));
            AddColumn("dbo.Semesters", "NextYear", c => c.Int(nullable: false));
            AddColumn("dbo.Semesters", "LastYear", c => c.Int(nullable: false));
            DropForeignKey("dbo.TotalScoreInYears", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.TotalScoreInYears", "IDYear", "dbo.SchoolYears");
            DropForeignKey("dbo.Class", "IDYear", "dbo.SchoolYears");
            DropForeignKey("dbo.SchoolYears", "IDOrganization", "dbo.Organization");
            DropIndex("dbo.TotalScoreInYears", new[] { "IDYear" });
            DropIndex("dbo.TotalScoreInYears", new[] { "IDStudent" });
            DropIndex("dbo.SchoolYears", new[] { "IDOrganization" });
            DropIndex("dbo.Class", new[] { "IDYear" });
            DropColumn("dbo.Class", "IDYear");
            DropTable("dbo.TotalScoreInYears");
            DropTable("dbo.SchoolYears");
            RenameIndex(table: "dbo.TotalScoreSubject", name: "IX_IDYear", newName: "IX_IDSemester");
            RenameColumn(table: "dbo.TotalScoreSubject", name: "IDYear", newName: "IDSemester");
            CreateIndex("dbo.Teach", "IDSemester");
            CreateIndex("dbo.Class", "IDSemester");
            AddForeignKey("dbo.Class", "IDSemester", "dbo.Semesters", "IdSemester", cascadeDelete: true);
            AddForeignKey("dbo.Teach", "IDSemester", "dbo.Semesters", "IdSemester", cascadeDelete: true);
        }
    }
}
