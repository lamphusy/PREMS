namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initdatabase : DbMigration
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
                        IsPaid = c.Boolean(nullable: false),
                        LogoPath = c.String(),
                        FacebookLink = c.String(),
                        InstagramLink = c.String(),
                        LinkedinLink = c.String(),
                    })
                .PrimaryKey(t => t.IdOrganization);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        IdSemester = c.Int(nullable: false, identity: true),
                        SemesterNum = c.Int(nullable: false),
                        IsNow = c.Boolean(nullable: false),
                        IDYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSemester)
                .ForeignKey("dbo.SchoolYears", t => t.IDYear, cascadeDelete: true)
                .Index(t => t.IDYear);
            
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
                "dbo.Student",
                c => new
                    {
                        IDStudent = c.String(nullable: false, maxLength: 128),
                        IDOrganization = c.String(maxLength: 128),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        AvatarPath = c.String(),
                        Gender = c.String(),
                    })
                .PrimaryKey(t => t.IDStudent)
                .ForeignKey("dbo.ApplicationUsers", t => t.IDStudent)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDStudent)
                .Index(t => t.IDOrganization);
            
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
                        IDYear = c.Int(nullable: false),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IDClass)
                .ForeignKey("dbo.Teacher", t => t.IDHomeroomTeacher)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .ForeignKey("dbo.SchoolYears", t => t.IDYear, cascadeDelete: true)
                .Index(t => t.IDHomeroomTeacher)
                .Index(t => t.IDYear)
                .Index(t => t.IDOrganization);
            
            CreateTable(
                "dbo.Teacher",
                c => new
                    {
                        IDUser = c.String(nullable: false, maxLength: 128),
                        IDOrganization = c.String(maxLength: 128),
                        IDCard = c.String(maxLength: 100),
                        CreateDate = c.DateTime(nullable: false),
                        StartJobDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        Degree = c.String(),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CoefficientsSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Specialization = c.String(),
                        AvatarPath = c.String(),
                        Gender = c.String(),
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
                        ID = c.Int(nullable: false, identity: true),
                        IDTeacher = c.String(maxLength: 128),
                        IDClass = c.String(maxLength: 128),
                        IDSchoolYear = c.Int(nullable: false),
                        IDPeriod = c.Int(nullable: false),
                        WeekDay = c.Int(nullable: false),
                        IDSubject = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Class", t => t.IDClass)
                .ForeignKey("dbo.OPeriodLessons", t => t.IDPeriod, cascadeDelete: true)
                .ForeignKey("dbo.SchoolYears", t => t.IDSchoolYear, cascadeDelete: true)
                .ForeignKey("dbo.Subject", t => t.IDSubject)
                .ForeignKey("dbo.Teacher", t => t.IDTeacher)
                .Index(t => t.IDTeacher)
                .Index(t => t.IDClass)
                .Index(t => t.IDSchoolYear)
                .Index(t => t.IDPeriod)
                .Index(t => t.IDSubject);
            
            CreateTable(
                "dbo.OPeriodLessons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDOrganization = c.String(maxLength: 128),
                        PeriodStartTime = c.Int(nullable: false),
                        PeriodEndTime = c.Int(nullable: false),
                        PeriodName = c.String(),
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
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDOrganization);
            
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
                        IDYear = c.Int(nullable: false),
                        Score = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SchoolYears", t => t.IDYear, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.IDStudent)
                .ForeignKey("dbo.Subject", t => t.IDSubject)
                .Index(t => t.IDStudent)
                .Index(t => t.IDSubject)
                .Index(t => t.IDYear);
            
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
                        Status = c.Int(nullable: false),
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
                "dbo.ORegister",
                c => new
                    {
                        IdApplicationUser = c.String(nullable: false, maxLength: 128),
                        IdCard = c.String(nullable: false),
                        RegisterDate = c.DateTime(nullable: false),
                        Nation = c.String(),
                    })
                .PrimaryKey(t => t.IdApplicationUser)
                .ForeignKey("dbo.ApplicationUsers", t => t.IdApplicationUser)
                .Index(t => t.IdApplicationUser);
            
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
                        BankCode = c.String(),
                        IDOrganization = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IDReceipt)
                .ForeignKey("dbo.ORegister", t => t.IDAccount)
                .ForeignKey("dbo.Organization", t => t.IDOrganization)
                .Index(t => t.IDAccount)
                .Index(t => t.IDOrganization);
            
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
                "dbo.UserOwnOrganizations",
                c => new
                    {
                        IdORegister = c.String(nullable: false, maxLength: 128),
                        IdOrganization = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.IdORegister, t.IdOrganization })
                .ForeignKey("dbo.ORegister", t => t.IdORegister, cascadeDelete: true)
                .ForeignKey("dbo.Organization", t => t.IdOrganization, cascadeDelete: true)
                .Index(t => t.IdORegister)
                .Index(t => t.IdOrganization);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserRoles", "IdentityRole_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.UserOwnOrganizations", "IdOrganization", "dbo.Organization");
            DropForeignKey("dbo.UserOwnOrganizations", "IdORegister", "dbo.ORegister");
            DropForeignKey("dbo.ScoreType", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.TotalScoreInYears", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.TotalScoreInYears", "IDYear", "dbo.SchoolYears");
            DropForeignKey("dbo.Receipt", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.Receipt", "IDAccount", "dbo.ORegister");
            DropForeignKey("dbo.ORegulation", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.ORegister", "IdApplicationUser", "dbo.ApplicationUsers");
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
            DropForeignKey("dbo.TotalScoreSubject", "IDYear", "dbo.SchoolYears");
            DropForeignKey("dbo.TotalScore", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.TotalScore", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Studies", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.Studies", "IDClass", "dbo.Class");
            DropForeignKey("dbo.Class", "IDYear", "dbo.SchoolYears");
            DropForeignKey("dbo.Class", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.Class", "IDHomeroomTeacher", "dbo.Teacher");
            DropForeignKey("dbo.Teach", "IDTeacher", "dbo.Teacher");
            DropForeignKey("dbo.Teach", "IDSubject", "dbo.Subject");
            DropForeignKey("dbo.Teach", "IDSchoolYear", "dbo.SchoolYears");
            DropForeignKey("dbo.Teach", "IDPeriod", "dbo.OPeriodLessons");
            DropForeignKey("dbo.OPeriodLessons", "IDShift", "dbo.OShift");
            DropForeignKey("dbo.OShift", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.OPeriodLessons", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.Teach", "IDClass", "dbo.Class");
            DropForeignKey("dbo.Teacher", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.Teacher", "IDUser", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ScoreDetail", "IDSubject", "dbo.Subject");
            DropForeignKey("dbo.Subject", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.ScoreDetail", "IDStudent", "dbo.Student");
            DropForeignKey("dbo.ScoreDetail", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Student", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.Student", "IDStudent", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.AbsenteeForm", "IDSemester", "dbo.Semesters");
            DropForeignKey("dbo.Semesters", "IDYear", "dbo.SchoolYears");
            DropForeignKey("dbo.SchoolYears", "IDOrganization", "dbo.Organization");
            DropForeignKey("dbo.AbsenteeForm", "IDOrganization", "dbo.Organization");
            DropIndex("dbo.UserOwnOrganizations", new[] { "IdOrganization" });
            DropIndex("dbo.UserOwnOrganizations", new[] { "IdORegister" });
            DropIndex("dbo.ScoreType", new[] { "IDOrganization" });
            DropIndex("dbo.TotalScoreInYears", new[] { "IDYear" });
            DropIndex("dbo.TotalScoreInYears", new[] { "IDStudent" });
            DropIndex("dbo.Receipt", new[] { "IDOrganization" });
            DropIndex("dbo.Receipt", new[] { "IDAccount" });
            DropIndex("dbo.ORegulation", new[] { "IDOrganization" });
            DropIndex("dbo.ORegister", new[] { "IdApplicationUser" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDSemester" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDNewClass" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDOldClass" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDStudent" });
            DropIndex("dbo.ClassTransferringForm", new[] { "IDOrganization" });
            DropIndex("dbo.AnnouncementTargets", new[] { "IDOrganization" });
            DropIndex("dbo.Announcement", new[] { "IDOrganization" });
            DropIndex("dbo.Announcement", new[] { "IDTarget" });
            DropIndex("dbo.Admin", new[] { "IDUser" });
            DropIndex("dbo.TotalScoreSubject", new[] { "IDYear" });
            DropIndex("dbo.TotalScoreSubject", new[] { "IDSubject" });
            DropIndex("dbo.TotalScoreSubject", new[] { "IDStudent" });
            DropIndex("dbo.TotalScore", new[] { "IDSemester" });
            DropIndex("dbo.TotalScore", new[] { "IDStudent" });
            DropIndex("dbo.OShift", new[] { "IDOrganization" });
            DropIndex("dbo.OPeriodLessons", new[] { "IDShift" });
            DropIndex("dbo.OPeriodLessons", new[] { "IDOrganization" });
            DropIndex("dbo.Teach", new[] { "IDSubject" });
            DropIndex("dbo.Teach", new[] { "IDPeriod" });
            DropIndex("dbo.Teach", new[] { "IDSchoolYear" });
            DropIndex("dbo.Teach", new[] { "IDClass" });
            DropIndex("dbo.Teach", new[] { "IDTeacher" });
            DropIndex("dbo.Teacher", new[] { "IDOrganization" });
            DropIndex("dbo.Teacher", new[] { "IDUser" });
            DropIndex("dbo.Class", new[] { "IDOrganization" });
            DropIndex("dbo.Class", new[] { "IDYear" });
            DropIndex("dbo.Class", new[] { "IDHomeroomTeacher" });
            DropIndex("dbo.Studies", new[] { "IDStudent" });
            DropIndex("dbo.Studies", new[] { "IDClass" });
            DropIndex("dbo.Subject", new[] { "IDOrganization" });
            DropIndex("dbo.ScoreDetail", new[] { "IDSemester" });
            DropIndex("dbo.ScoreDetail", new[] { "IDSubject" });
            DropIndex("dbo.ScoreDetail", new[] { "IDStudent" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Student", new[] { "IDOrganization" });
            DropIndex("dbo.Student", new[] { "IDStudent" });
            DropIndex("dbo.SchoolYears", new[] { "IDOrganization" });
            DropIndex("dbo.Semesters", new[] { "IDYear" });
            DropIndex("dbo.AbsenteeForm", new[] { "IDOrganization" });
            DropIndex("dbo.AbsenteeForm", new[] { "IDSemester" });
            DropIndex("dbo.AbsenteeForm", new[] { "IDStudent" });
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.UserOwnOrganizations");
            DropTable("dbo.ScoreType");
            DropTable("dbo.TotalScoreInYears");
            DropTable("dbo.Statistics");
            DropTable("dbo.Receipt");
            DropTable("dbo.ORegulation");
            DropTable("dbo.ORegister");
            DropTable("dbo.ClassTransferringForm");
            DropTable("dbo.AnnouncementTargets");
            DropTable("dbo.Announcement");
            DropTable("dbo.Admin");
            DropTable("dbo.TotalScoreSubject");
            DropTable("dbo.TotalScore");
            DropTable("dbo.OShift");
            DropTable("dbo.OPeriodLessons");
            DropTable("dbo.Teach");
            DropTable("dbo.Teacher");
            DropTable("dbo.Class");
            DropTable("dbo.Studies");
            DropTable("dbo.Subject");
            DropTable("dbo.ScoreDetail");
            DropTable("dbo.ApplicationUserRoles");
            DropTable("dbo.ApplicationUserLogins");
            DropTable("dbo.ApplicationUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.Student");
            DropTable("dbo.SchoolYears");
            DropTable("dbo.Semesters");
            DropTable("dbo.Organization");
            DropTable("dbo.AbsenteeForm");
        }
    }
}
