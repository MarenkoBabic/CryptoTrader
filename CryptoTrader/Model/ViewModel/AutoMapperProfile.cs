﻿namespace CryptoTrader.Models.ViewModel
{
    using AutoMapper;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            using (var db = new JaroshEntities())
            {
                #region Register / Login
                //RegisterViewModel
                CreateMap<Person, RegisterViewModel>()
                    .ForMember(a => a.RegisterEmail, opt => opt.MapFrom(a => a.email))
                    .ForMember(a => a.RegisterPassword, opt => opt.MapFrom(a => a.password));
                CreateMap<RegisterViewModel, Person>()
                    .ForMember(a => a.email, opt => opt.MapFrom(a => a.RegisterEmail))
                    .ForMember(a => a.password, opt => opt.MapFrom(a => a.RegisterPassword));

                //LoginViewModel
                CreateMap<Person, LoginViewModel>()
                    .ForMember(a => a.LoginEmail, opt => opt.MapFrom(a => a.email))
                    .ForMember(a => a.LoginPassword, opt => opt.MapFrom(a => a.password));
                CreateMap<LoginViewModel, Person>()
                    .ForMember(a => a.email, opt => opt.MapFrom(a => a.LoginEmail))
                    .ForMember(a => a.password, opt => opt.MapFrom(a => a.LoginPassword));
                #endregion

                #region PersonVerificationViewModel

                //Person
                CreateMap<Person, PersonVerificationViewModel>();
                CreateMap<PersonVerificationViewModel, Person>();

                //Country
                CreateMap<PersonVerificationViewModel, Country>()
                    .ForMember(a => a.id, opt => opt.MapFrom(a => a.Country_id));

                //City
                CreateMap<City, PersonVerificationViewModel>()
                    .ForMember(a => a.City_id, opt => opt.MapFrom(a => a.id));

                CreateMap<PersonVerificationViewModel, City>()
                    .ForMember(a => a.id, opt => opt.MapFrom(a => a.City_id))
                    .ForMember(a => a.country_id, opt => opt.MapFrom(src => Mapper.Map<PersonVerificationViewModel>(src)));

                //Adress
                CreateMap<Address, PersonVerificationViewModel>()
                    .ForMember(a => a.Address_id, opt => opt.MapFrom(a => a.id));
                CreateMap<PersonVerificationViewModel, Address>()
                    .ForMember(a => a.id, opt => opt.MapFrom(a => a.Address_id));



                //Upload
                CreateMap<Upload, PersonVerificationViewModel>();
                CreateMap<PersonVerificationViewModel, Upload>();

                #endregion


                #region BankTransfer
                //PayIn    
                CreateMap<PayInViewModel, Person>();
                CreateMap<Person, PayInViewModel>();

                //PayOut
                CreateMap<BankTransferHistory, PayOutViewModel>();
                CreateMap<PayOutViewModel, BankTransferHistory>();

                CreateMap<PayOutViewModel, Person>();
                CreateMap<Person, PayOutViewModel>();

                CreateMap<BankAccount, PayOutViewModel>()
                    .ForMember(a => a.PersonBic, opt => opt.MapFrom(a => a.bic))
                    .ForMember(a => a.PersonIban, opt => opt.MapFrom(a => a.iban));
                CreateMap<PayOutViewModel, BankAccount>()
                    .ForMember(a => a.bic, opt => opt.MapFrom(a => a.PersonBic))
                    .ForMember(a => a.iban, opt => opt.MapFrom(a => a.PersonIban));

                CreateMap<PayOutViewModel, Balance>();

                #endregion

                #region TradeBTC

                CreateMap<TradeHistory, SellBitCoinViewModel>()
                    .ForMember(a => a.PersonId, opt => opt.MapFrom(a => a.person_id))
                    .ForMember(a => a.TradeAmount, opt => opt.MapFrom(a => a.amount))
                    .ForMember(a => a.TickerId, opt => opt.MapFrom(a => a.ticker_id));

                CreateMap<SellBitCoinViewModel, TradeHistory>()
                    .ForMember(a => a.person_id, opt => opt.MapFrom(a => a.PersonId))
                    .ForMember(a => a.amount, opt => opt.MapFrom(a => a.TradeAmount))
                    .ForMember(a => a.ticker_id, opt => opt.MapFrom(a => a.TickerId));

                CreateMap<Ticker, ApiViewModel>();
                CreateMap<ApiViewModel, Ticker>();

                #endregion



            }
        }
    }
}
