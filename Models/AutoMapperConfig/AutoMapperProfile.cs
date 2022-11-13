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

        
        }
    }
}
