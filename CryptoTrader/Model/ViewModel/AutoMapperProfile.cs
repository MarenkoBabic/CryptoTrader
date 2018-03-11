namespace CryptoTrader.Models.ViewModel
{
    using AutoMapper;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            using (var db = new CryptoEntities())
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


                #region MyRegion
                //BankDataViewModel
                CreateMap<BankTransferHistory, BankDataViewModel>()
                    .ForMember(a => a.Created, opt => opt.MapFrom(a => a.created))
                    .ForMember(a => a.Currency, opt => opt.MapFrom(a => a.currency))
                    .ForMember(a => a.Amount, opt => opt.MapFrom(a => a.amount));
                CreateMap<BankDataViewModel, BankTransferHistory>()
                    .ForMember(a => a.created, opt => opt.MapFrom(a => a.Created))
                    .ForMember(a => a.currency, opt => opt.MapFrom(a => a.Currency))
                    .ForMember(a => a.amount, opt => opt.MapFrom(a => a.Amount));

                CreateMap<BankDataViewModel, Person>()
                    .ForMember(a => a.firstName, opt => opt.MapFrom(a => a.FirstName))
                    .ForMember(a => a.lastName, opt => opt.MapFrom(a => a.LastName));

                CreateMap<Person, BankDataViewModel>()
                .ForMember(a => a.FirstName, opt => opt.MapFrom(a => a.firstName))
                .ForMember(a => a.LastName, opt => opt.MapFrom(a => a.lastName));

                CreateMap<BankAccount, BankDataViewModel>()
                    .ForMember(a => a.PersonBic, opt => opt.MapFrom(a => a.bic))
                    .ForMember(a => a.PersonIban, opt => opt.MapFrom(a => a.iban));
                CreateMap<BankDataViewModel, BankAccount>()
                    .ForMember(a => a.bic, opt => opt.MapFrom(a => a.PersonBic))
                    .ForMember(a => a.iban, opt => opt.MapFrom(a => a.PersonIban));


                CreateMap<TradeHistory, TradeViewModel>()
                    .ForMember(a => a.Created, opt => opt.MapFrom(a => a.created))
                    .ForMember(a => a.PersonId, opt => opt.MapFrom(a => a.person_id))
                    .ForMember(a => a.TradeAmountBTC, opt => opt.MapFrom(a => a.amount))
                    .ForMember(a => a.TickerId, opt => opt.MapFrom(a => a.ticker_id));

                CreateMap<TradeViewModel, TradeHistory>()
                    .ForMember(a => a.created, opt => opt.MapFrom(a => a.Created))
                    .ForMember(a => a.person_id, opt => opt.MapFrom(a => a.PersonId))
                    .ForMember(a => a.amount, opt => opt.MapFrom(a => a.TradeAmountBTC))
                    .ForMember(a => a.ticker_id, opt => opt.MapFrom(a => a.TickerId));

                CreateMap<Ticker, ApiViewModel>();
                CreateMap<ApiViewModel, Ticker>();

                #endregion


            }
        }
    }
}
