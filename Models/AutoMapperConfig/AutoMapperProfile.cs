using AutoMapper;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AutoMapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ORegister, UserViewModel>()
                .ForMember(x => x.EmailAddress, src => src.MapFrom(a => a.ApplicationUser.Email))
                .ForMember(x => x.FullName, src => src.MapFrom(a => a.ApplicationUser.FullName))
                .ForMember(x => x.Address, src => src.MapFrom(a => a.ApplicationUser.Address))
                .ForMember(x => x.DayOfBirth, src => src.MapFrom(a => a.ApplicationUser.DayOfBirth))
                .ForMember(x => x.PhoneNumber, src => src.MapFrom(a => a.ApplicationUser.PhoneNumber))
                .ForMember(x => x.Username, src => src.MapFrom(a => a.ApplicationUser.UserName));

            CreateMap<Receipt, ReceiptViewModel>()
                .ForMember(x => x.AccountName, src => src.MapFrom(a => a.ORegister.ApplicationUser.FullName))
                .ForMember(x => x.AccountUsername, src => src.MapFrom(a => a.ORegister.ApplicationUser.UserName))
                .ForMember(x => x.OrganizationName, src => src.MapFrom(a => a.Organization.Name));

            CreateMap<Organization, OrganizationViewModel>();
            CreateMap<OShift, OShiftViewModel>();
            CreateMap<OPeriodLesson, OPeriodLessonViewModel>()
                .ForMember(x => x.PeriodID, src => src.MapFrom(a=> a.ID));
            CreateMap<Semester, SemesterViewModel>();

            CreateMap<Teacher, TeacherViewModel>()
                .ForMember(x => x.FullName, src => src.MapFrom(a => a.ApplicationUser.FullName))
                .ForMember(x => x.DayOfBirth, src => src.MapFrom(a => a.ApplicationUser.DayOfBirth))
                .ForMember(x => x.Address, src => src.MapFrom(a => a.ApplicationUser.Address))
                .ForMember(x => x.Username, src => src.MapFrom(a => a.ApplicationUser.UserName))
                .ForMember(x => x.PhoneNumber, src => src.MapFrom(a => a.ApplicationUser.PhoneNumber))
                .ForMember(x => x.Email, src => src.MapFrom(a => a.ApplicationUser.Email));

            CreateMap<Student, StudentViewModel>()
                .ForMember(x => x.FullName, src => src.MapFrom(a => a.ApplicationUser.FullName))
                .ForMember(x => x.DayOfBirth, src => src.MapFrom(a => a.ApplicationUser.DayOfBirth))
                .ForMember(x => x.Address, src => src.MapFrom(a => a.ApplicationUser.Address))
                .ForMember(x => x.Username, src => src.MapFrom(a => a.ApplicationUser.UserName))
                .ForMember(x => x.PhoneNumber, src => src.MapFrom(a => a.ApplicationUser.PhoneNumber))
                .ForMember(x => x.Email, src => src.MapFrom(a => a.ApplicationUser.Email))
                .ForMember(x => x.Class, src => src.MapFrom(a => a.Studies.OrderByDescending(x => x.IDStudy).FirstOrDefault().Class.Name));

            CreateMap<Study, StudyViewModel>()
                .ForMember(x => x.StudentName, src => src.MapFrom(a => a.Student.ApplicationUser.FullName))
                .ForMember(x => x.StudentGender, src => src.MapFrom(a => a.Student.Gender))
                .ForMember(x => x.StudentBirth, src => src.MapFrom(a => a.Student.ApplicationUser.DayOfBirth.Value.ToString("d")));

            CreateMap<Teach, TeachViewModel>()
                .ForMember(x => x.SubjectName, src => src.MapFrom(a => a.Subject.SubjectName))
                .ForMember(x => x.TeacherName, src => src.MapFrom(a => a.Teacher.ApplicationUser.FullName))
                .ForMember(x => x.ClassName, src => src.MapFrom(a => a.Class.Name));

        }
    }
}
