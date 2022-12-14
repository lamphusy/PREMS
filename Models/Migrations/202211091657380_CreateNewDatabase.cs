namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbsenteeForm",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        IDStudent = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Shift = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        IDSemester = c.Int(nullable: false),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .ForeignKey("dbo.Semesters", t => t.IDSemester, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.IDStudent)
                .Index(t => t.IDStudent)
                .Index(t => t.IDSemester)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        IdOrganization = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        CreateDate = c.DateTime(),
                        CreateBy = c.String(),
                        Information = c.String(),
                    })
                .PrimaryKey(t => t.IdOrganization);
            
            CreateTable(
                "dbo.ORegister",
                c => new
                    {
                        IdApplicationUser = c.String(nullable: false, maxLength: 128),
                        IdCard = c.String(nullable: false),
                        RegisterDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdApplicationUser)
                .ForeignKey("dbo.ApplicationUsers", t => t.IdApplicationUser)
                .Index(t => t.IdApplicationUser);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(maxLength: 256),
                        Address = c.String(maxLength: 256),
                        DayOfBirth = c.DateTime(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserClaims",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.ApplicationRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        IdSemester = c.Int(nullable: false, identity: true),
                        SemesterNum = c.Int(nullable: false),
                        LastYear = c.Int(nullable: false),
                        NextYear = c.Int(nullable: false),
                        IsNow = c.Boolean(nullable: false),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdSemester)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        IDStudent = c.String(nullable: false, maxLength: 128),
                        IDOrganization = c.String(maxLength: 128),
                        FullName = c.String(nullable: false),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        AvatarPath = c.String(),
                        Address = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(),
                    })
                .PrimaryKey(t => t.IDStudent)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.ScoreDetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDStudent = c.String(maxLength: 128),
                        IDSubject = c.String(maxLength: 128),
                        IDSemester = c.Int(nullable: false),
                        IDScoreType = c.String(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Semesters", t => t.IDSemester, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.IDStudent)
                .ForeignKey("dbo.Subject", t => t.IDSubject)
                .Index(t => t.IDStudent)
                .Index(t => t.IDSubject)
                .Index(t => t.IDSemester);
            
            CreateTable(
                "dbo.Subject",
                c => new
                    {
                        IDSubject = c.String(nullable: false, maxLength: 128),
                        SubjectName = c.String(),
                        Description = c.String(),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IDSubject)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.Studies",
                c => new
                    {
                        IDStudy = c.Int(nullable: false, identity: true),
                        IDClass = c.String(maxLength: 128),
                        IDStudent = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IDStudy)
                .ForeignKey("dbo.Class", t => t.IDClass)
                .ForeignKey("dbo.Student", t => t.IDStudent)
                .Index(t => t.IDClass)
                .Index(t => t.IDStudent);
            
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        IDClass = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        IDHomeroomTeacher = c.String(maxLength: 128),
                        Total = c.Int(),
                        IDSemester = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDClass)
                .ForeignKey("dbo.Teacher", t => t.IDHomeroomTeacher)
                .ForeignKey("dbo.Semesters", t => t.IDSemester, cascadeDelete: true)
                .Index(t => t.IDHomeroomTeacher)
                .Index(t => t.IDSemester);
            
            CreateTable(
                "dbo.Teacher",
                c => new
                    {
                        IDUser = c.String(nullable: false, maxLength: 128),
                        IDOrganization = c.String(maxLength: 128),
                        IDCard = c.String(maxLength: 100),
                        CreateDate = c.String(),
                        CreateBy = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Degree = c.String(),
                        Address = c.String(),
                        Gender = c.String(),
                        Salary = c.Double(nullable: false),
                        CoefficientsSalary = c.Double(nullable: false),
                        Specialization = c.String(),
                        AvatarPath = c.String(),
                    })
                .PrimaryKey(t => t.IDUser)
                .ForeignKey("dbo.ApplicationUsers", t => t.IDUser)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDUser)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.Teach",
                c => new
                    {
                        IDTeach = c.Int(nullable: false, identity: true),
                        IDTeacher = c.String(maxLength: 128),
                        IDClass = c.String(maxLength: 128),
                        IDSubject = c.String(maxLength: 128),
                        IDSemester = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDTeach)
                .ForeignKey("dbo.Class", t => t.IDClass)
                .ForeignKey("dbo.Semesters", t => t.IDSemester, cascadeDelete: true)
                .ForeignKey("dbo.Subject", t => t.IDSubject)
                .ForeignKey("dbo.Teacher", t => t.IDTeacher)
                .Index(t => t.IDTeacher)
                .Index(t => t.IDClass)
                .Index(t => t.IDSubject)
                .Index(t => t.IDSemester);
            
            CreateTable(
                "dbo.TotalScore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IDStudent = c.String(maxLength: 128),
                        IDSemester = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Semesters", t => t.IDSemester, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.IDStudent)
                .Index(t => t.IDStudent)
                .Index(t => t.IDSemester);
            
            CreateTable(
                "dbo.TotalScoreSubject",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDStudent = c.String(maxLength: 128),
                        IDSubject = c.String(maxLength: 128),
                        IDSemester = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Semesters", t => t.IDSemester, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.IDStudent)
                .ForeignKey("dbo.Subject", t => t.IDSubject)
                .Index(t => t.IDStudent)
                .Index(t => t.IDSubject)
                .Index(t => t.IDSemester);
            
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        IDUser = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.IDUser)
                .ForeignKey("dbo.ApplicationUsers", t => t.IDUser)
                .Index(t => t.IDUser);
            
            CreateTable(
                "dbo.Announcement",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        IDTarget = c.Int(nullable: false),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AnnouncementTargets", t => t.IDTarget, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDTarget)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.AnnouncementTargets",
                c => new
                    {
                        IDTarget = c.Int(nullable: false, identity: true),
                        Meaning = c.String(nullable: false),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IDTarget)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.ClassTransferringForm",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDOrganization = c.String(maxLength: 128),
                        IDStudent = c.String(maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        IDOldClass = c.String(maxLength: 128),
                        IDNewClass = c.String(maxLength: 128),
                        IDSemester = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Class", t => t.IDNewClass)
                .ForeignKey("dbo.Class", t => t.IDOldClass)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .ForeignKey("dbo.Semesters", t => t.IDSemester, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.IDStudent)
                .Index(t => t.IDOrganization)
                .Index(t => t.IDStudent)
                .Index(t => t.IDOldClass)
                .Index(t => t.IDNewClass)
                .Index(t => t.IDSemester);
            
            CreateTable(
                "dbo.OPeriodLessons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDOrganization = c.String(maxLength: 128),
                        PeriodStartTime = c.DateTime(nullable: false),
                        PeriodEndTime = c.DateTime(nullable: false),
                        IDShift = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .ForeignKey("dbo.OShift", t => t.IDShift, cascadeDelete: true)
                .Index(t => t.IDOrganization)
                .Index(t => t.IDShift);
            
            CreateTable(
                "dbo.OShift",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDOrganization = c.String(maxLength: 128),
                        ShiftName = c.String(nullable: false),
                        ShiftStartTime = c.DateTime(nullable: false),
                        ShiftEndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.ORegulation",
                c => new
                    {
                        IDOrganization = c.String(nullable: false, maxLength: 128),
                        NumberOfShift = c.Int(nullable: false),
                        NumberOfPeriod = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDOrganization)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.Receipt",
                c => new
                    {
                        IDReceipt = c.String(nullable: false, maxLength: 128),
                        IDAccount = c.String(maxLength: 128),
                        PaymentDate = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IDReceipt)
                .ForeignKey("dbo.ORegister", t => t.IDAccount)
                .Index(t => t.IDAccount);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Profit = c.Double(nullable: false),
                        NumOfRegister = c.Int(nullable: false),
                        NumOfOrganinze = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TeachDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDTeach = c.Int(nullable: false),
                        IDShift = c.Int(nullable: false),
                        Period = c.Int(nullable: false),
                        WeekDay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OShift", t => t.IDShift, cascadeDelete: true)
                .ForeignKey("dbo.Teach", t => t.IDTeach, cascadeDelete: true)
                .Index(t => t.IDTeach)
                .Index(t => t.IDShift);
            
            CreateTable(
                "dbo.ScoreType",
                c => new
                    {
                        IDScoreType = c.String(nullable: false, maxLength: 128),
                        NameScore = c.String(nullable: false),
                        PercentScore = c.Single(nullable: false),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IDScoreType)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ORegisterOrganizations",
                c => new
                    {
                        ORegister_IdApplicationUser = c.String(nullable: false, maxLength: 128),
                        Organization_IdOrganization = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ORegister_IdApplicationUser, t.Organization_IdOrganization })
                .ForeignKey("dbo.ORegister", t => t.ORegister_IdApplicationUser, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.Organization_IdOrganization, cascadeDelete: true)
                .Index(t => t.ORegister_IdApplicationUser)
                .Index(t => t.Organization_IdOrganization);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserRoles", "IdentityRole_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ScoreType", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.TeachDetails", "IDTeach", "dbo.Teach");
            DropForeignKey("dbo.TeachDetails", "IDShift", "dbo.OShift");
            DropForeignKey("dbo.Receipt", "IDAccount", "dbo.ORegister");
            DropForeignKey("dbo.ORegulation", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.OPeriodLessons", "IDShift", "dbo.OShift");
            DropForeignKey("dbo.OShift", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.OPeriodLessons", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.ClassTransferringForm", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.ClassTransferringForm", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.ClassTransferringForm", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.ClassTransferringForm", "IDOldClass", "dbo.Class");
            DropForeignKey("dbo.ClassTransferringForm", "IDNewClass", "dbo.Class");
            DropForeignKey("dbo.Announcement", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.Announcement", "IDTarget", "dbo.AnnouncementTargets");
            DropForeignKey("dbo.AnnouncementTargets", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.Admin", "IDUser", "dbo.ApplicationUsers");
            DropForeignKey("dbo.AbsenteeForm", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.TotalScoreSubject", "IDSubject", "dbo.Subject");
            DropForeignKey("dbo.TotalScoreSubject", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.TotalScoreSubject", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.TotalScore", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.TotalScore", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Studies", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.Studies", "IDClass", "dbo.Class");
            DropForeignKey("dbo.Class", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Class", "IDHomeroomTeacher", "dbo.Teacher");
            DropForeignKey("dbo.Teach", "IDTeacher", "dbo.Teacher");
            DropForeignKey("dbo.Teach", "IDSubject", "dbo.Subject");
            DropForeignKey("dbo.Teach", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Teach", "IDClass", "dbo.Class");
            DropForeignKey("dbo.Teacher", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.Teacher", "IDUser", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ScoreDetail", "IDSubject", "dbo.Subject");
            DropForeignKey("dbo.Subject", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.ScoreDetail", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.ScoreDetail", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Student", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.AbsenteeForm", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Semesters", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.AbsenteeForm", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.ORegisterOrganizations", "Organization_IdOrganization", "dbo.Organization");
            DropForeignKey("dbo.ORegisterOrganizations", "ORegister_IdApplicationUser", "dbo.ORegister");
            DropForeignKey("dbo.ORegister", "IdApplicationUser", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.ORegisterOrganizations", new[] { "Organization_IdOrganization" });
            DropIndex("dbo.ORegisterOrganizations", new[] { "ORegister_IdApplicationUser" });
            DropIndex("dbo.ScoreType", new[] { "IDOrganization" });
            DropIndex("dbo.TeachDetails", new[] { "IDShift" });
            DropIndex("dbo.TeachDetails", new[] { "IDTeach" });
            DropIndex("dbo.Receipt", new[] { "IDAccount" });
            DropIndex("dbo.ORegulation", new[] { "IDOrganization" });
            DropIndex("dbo.OShift", new[] { "IDOrganization" });
            DropIndex("dbo.OPeriodLessons", new[] { "IDShift" });
            DropIndex("dbo.OPeriodLessons", new[] { "IDOrganization" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDSemester" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDNewClass" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDOldClass" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDStudent" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDOrganization" });
            DropIndex("dbo.AnnouncementTargets", new[] { "IDOrganization" });
            DropIndex("dbo.Announcement", new[] { "IDOrganization" });
            DropIndex("dbo.Announcement", new[] { "IDTarget" });
            DropIndex("dbo.Admin", new[] { "IDUser" });
            DropIndex("dbo.TotalScoreSubject", new[] { "IDSemester" });
            DropIndex("dbo.TotalScoreSubject", new[] { "IDSubject" });
            DropIndex("dbo.TotalScoreSubject", new[] { "IDStudent" });
            DropIndex("dbo.TotalScore", new[] { "IDSemester" });
            DropIndex("dbo.TotalScore", new[] { "IDStudent" });
            DropIndex("dbo.Teach", new[] { "IDSemester" });
            DropIndex("dbo.Teach", new[] { "IDSubject" });
            DropIndex("dbo.Teach", new[] { "IDClass" });
            DropIndex("dbo.Teach", new[] { "IDTeacher" });
            DropIndex("dbo.Teacher", new[] { "IDOrganization" });
            DropIndex("dbo.Teacher", new[] { "IDUser" });
            DropIndex("dbo.Class", new[] { "IDSemester" });
            DropIndex("dbo.Class", new[] { "IDHomeroomTeacher" });
            DropIndex("dbo.Studies", new[] { "IDStudent" });
            DropIndex("dbo.Studies", new[] { "IDClass" });
            DropIndex("dbo.Subject", new[] { "IDOrganization" });
            DropIndex("dbo.ScoreDetail", new[] { "IDSemester" });
            DropIndex("dbo.ScoreDetail", new[] { "IDSubject" });
            DropIndex("dbo.ScoreDetail", new[] { "IDStudent" });
            DropIndex("dbo.Student", new[] { "IDOrganization" });
            DropIndex("dbo.Semesters", new[] { "IDOrganization" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ORegister", new[] { "IdApplicationUser" });
            DropIndex("dbo.AbsenteeForm", new[] { "IDOrganization" });
            DropIndex("dbo.AbsenteeForm", new[] { "IDSemester" });
            DropIndex("dbo.AbsenteeForm", new[] { "IDStudent" });
            DropTable("dbo.ORegisterOrganizations");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.ScoreType");
            DropTable("dbo.TeachDetails");
            DropTable("dbo.Statistics");
            DropTable("dbo.Receipt");
            DropTable("dbo.ORegulation");
            DropTable("dbo.OShift");
            DropTable("dbo.OPeriodLessons");
            DropTable("dbo.ClassTransferringForm");
            DropTable("dbo.AnnouncementTargets");
            DropTable("dbo.Announcement");
            DropTable("dbo.Admin");
            DropTable("dbo.TotalScoreSubject");
            DropTable("dbo.TotalScore");
            DropTable("dbo.Teach");
            DropTable("dbo.Teacher");
            DropTable("dbo.Class");
            DropTable("dbo.Studies");
            DropTable("dbo.Subject");
            DropTable("dbo.ScoreDetail");
            DropTable("dbo.Student");
            DropTable("dbo.Semesters");
            DropTable("dbo.ApplicationUserRoles");
            DropTable("dbo.ApplicationUserLogins");
            DropTable("dbo.ApplicationUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.ORegister");
            DropTable("dbo.Organization");
            DropTable("dbo.AbsenteeForm");
        }
    }
}
