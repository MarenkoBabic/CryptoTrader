using AutoMapper;
using CryptoTrader.Models.DbModel;

namespace CryptoTrader.Models.ViewModel
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            using (var db = new CryptoEntities())
            {
                CreateMap<Person, RegisterViewModel>()
                    .ForMember(a => a.RegisterEmail, opt => opt.MapFrom(a => a.email))
                    .ForMember(a => a.RegisterPassword, opt => opt.MapFrom(a => a.password));
                CreateMap<RegisterViewModel, Person>()
                    .ForMember(a => a.email, opt => opt.MapFrom(a => a.RegisterEmail))
                    .ForMember(a => a.password, opt => opt.MapFrom(a => a.RegisterPassword));

                CreateMap<Person, LoginViewModel>()
                    .ForMember(a => a.LoginEmail, opt => opt.MapFrom(a => a.email))
                    .ForMember(a => a.LoginPassword, opt => opt.MapFrom(a => a.password));
                CreateMap<LoginViewModel, Person>()
                    .ForMember(a => a.email, opt => opt.MapFrom(a => a.LoginEmail))
                    .ForMember(a => a.password, opt => opt.MapFrom(a => a.LoginPassword));

                CreateMap<Person, PersonVerificationViewModel>();
                CreateMap<PersonVerificationViewModel, Person>();


                CreateMap<Address, PersonVerificationViewModel>()
                    .ForMember(a => a.Address_id, opt => opt.MapFrom(a => a.id));
                CreateMap<PersonVerificationViewModel, Address>()
                    .ForMember(a => a.id, opt => opt.MapFrom(a => a.Address_id));


                CreateMap<City, PersonVerificationViewModel>()
                    .ForMember(a => a.City_id, opt => opt.MapFrom(a => a.id));
                CreateMap<PersonVerificationViewModel, City>()
                    .ForMember(a => a.id, opt => opt.MapFrom(a => a.City_id));


                CreateMap<Country, PersonVerificationViewModel>();
                CreateMap<PersonVerificationViewModel, Country>();


                CreateMap<Upload, PersonVerificationViewModel>();
                CreateMap<PersonVerificationViewModel, Upload>();

            }

        }
    }
}