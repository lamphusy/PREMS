using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Models
{
    public class SchoolManagementDbContext : DbContext
    {
        
        public SchoolManagementDbContext()
            : base("name=SchoolManagementDbContext")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;

        }

        public static SchoolManagementDbContext Create()
        {
            return new SchoolManagementDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            modelBuilder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Statistics> Statistics { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<ORegister> ORegister { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<AnnouncementTarget> AnnouncementTargets { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<ORegulation> ORegulations { get; set; }
        public virtual DbSet<OShift> OShifts { get; set; }
        public virtual DbSet<OPeriodLesson> OPeriodLessons { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teach> Teaches { get; set; }
        //public virtual DbSet<TeachDetail> TeachDetails { get; set; }
        public virtual DbSet<Study> Studies { get; set; }
        public virtual DbSet<ClassTransferringForm> ClassTransferringForms { get; set; }
        public virtual DbSet<TypeScore> TypeScores { get; set; }
        public virtual DbSet<ScoreDetail> ScoreDetails { get; set; }
        public virtual DbSet<TotalScoreSubject> TotalScoreSubjects { get; set; }
        public virtual DbSet<TotalScoreInSemester> TotalScoreInSemesters { get; set; }
        public virtual DbSet<TotalScoreInYear> TotalScoreInYears { get; set; }
        public virtual DbSet<AbsenteeForm> AbsenteeForms { get; set; }
        public virtual DbSet<UserOwnOrganization> UserOwnOrganizations { get; set; }
        public virtual DbSet<SchoolYear> SchoolYears { get; set; }
    }



}