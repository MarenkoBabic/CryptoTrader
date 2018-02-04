using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CryptoTrader.Models.DbModel;

namespace CryptoTrader.Models.ViewModel
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            using( var db = new CryptoTraderEntities() )
            {
                CreateMap<Person, RegisterViewModel>()
                    .ForMember( a => a.RegisterEmail, conf => conf.MapFrom( a => a.email ) )
                    .ForMember( a => a.RegisterPassword, conf => conf.MapFrom( a => a.password ) );
                CreateMap<RegisterViewModel, Person>()
                    .ForMember( a => a.email, conf => conf.MapFrom( a => a.RegisterEmail ) )
                    .ForMember( a => a.password, conf => conf.MapFrom( a => a.RegisterPassword ) );

                CreateMap<Person, LoginViewModel>()
                    .ForMember( a => a.LoginEmail, conf => conf.MapFrom( a => a.email ) )
                    .ForMember( a => a.LoginPassword, conf => conf.MapFrom( a => a.password ) );
                CreateMap<LoginViewModel, Person>()
                    .ForMember( a => a.email, conf => conf.MapFrom( a => a.LoginEmail ) )
                    .ForMember( a => a.password, conf => conf.MapFrom( a => a.LoginPassword ) );

                CreateMap<Person, PersonVerificationViewModel>();
                CreateMap<PersonVerificationViewModel, Person>();


                CreateMap<Address, PersonVerificationViewModel>()
                    .ForMember( a => a.Address_id,conf => conf.MapFrom(a => a.id));
                CreateMap<PersonVerificationViewModel, Address>()
                    .ForMember(a => a.id,conf => conf.MapFrom(a => a.Address_id));


                CreateMap<City, PersonVerificationViewModel>()
                    .ForMember( a => a.City_id, conf => conf.MapFrom( a => a.id ) );
                CreateMap<PersonVerificationViewModel, City>()
                    .ForMember( a => a.id, conf => conf.MapFrom( a => a.City_id ) );


                CreateMap<Country, PersonVerificationViewModel>();
                CreateMap<PersonVerificationViewModel, Country>();


                CreateMap<Upload, PersonVerificationViewModel>();
                CreateMap<PersonVerificationViewModel, Upload>();


            }

        }
    }
}