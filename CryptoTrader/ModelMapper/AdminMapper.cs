namespace CryptoTrader.ModelMapper
{
    using AutoMapper;
    using CryptoTrader.Model.DbModel;
    using CryptoTrader.Model.ViewModel;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class AdminMapper
    {
        AdminViewModel adminVM = new AdminViewModel();
        public static List<AdminViewModel> PersonList()
        {
            using (var db = new CryptoTraderEntities())
            {
                List<Person> dbPerson = db.Person.ToList();
                List<AdminViewModel> list = Mapper.Map<List<Person>, List<AdminViewModel>>(dbPerson);
                return list;
            }
        }

        public static void ChangeActiveStatus(int id)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);

                if (dbPerson.active == true)
                {
                    dbPerson.active = false;
                }
                else
                {
                    dbPerson.active = true;
                }
                db.Entry(dbPerson).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static List<AdminViewModel> FilterThePersonList(int id,string firstName,string lastName,string reference)
        {
            List<AdminViewModel> list = PersonList();

            if (id > 0)
            {
                list = list.Where(a => a.PersonId == id).ToList();
            }
            if (HasValue(firstName))
            {
                list = list.Where(a => a.FirstName.StartsWith(firstName, System.StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            if (HasValue(lastName))
            {
                list = list.Where(a => a.LastName.StartsWith(lastName, System.StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            if (HasValue(reference))
            {
                list = list.Where(a => a.Reference.StartsWith(reference, System.StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            return list;
        }

        /// <summary>
        /// Validiert den mitgebenen String
        /// </summary>
        /// <param name="s">ViewModel property</param>
        /// <returns>bool</returns>
        private static bool HasValue(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return true;
            }
            return false;
        }

    }
}
