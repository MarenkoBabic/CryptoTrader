using AutoMapper;
using CryptoTrader.Model.DbModel;
using CryptoTrader.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CryptoTrader.Manager
{
    public class AdminManager
    {
        /// <summary>
        /// Ändert den Aktivierungstatus des Users
        /// </summary>
        /// <param name="id">Person_id</param>
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

        /// <summary>
        /// Filterd die PersonenListe
        /// </summary>
        /// <param name="id">adminVM_PersonId</param>
        /// <param name="firstName">adminVM_Vorname</param>
        /// <param name="lastName">adminVM_Nachname</param>
        /// <param name="reference">adminVM_Verwendungszweck</param>
        /// <returns>Gefilterte PersonenListe</returns>
        public static List<AdminViewModel> FilterThePersonList(int id, string firstName, string lastName, string reference)
        {
            List<AdminViewModel> list = FillList.GetPersonList();

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
        /// Admin übernimmt das UserKonto
        /// </summary>
        /// <param name="id">Person_id</param>
        public static void GetUserLogin(int id)
        {
            using (var db = new CryptoTraderEntities())
            {
                Person dbPerson = db.Person.Find(id);
                string email = dbPerson.email;
                string firstName = dbPerson.firstName;
                string lastName = dbPerson.lastName;
                string role = dbPerson.role;
                Cookies.CreateCookies(email, role, firstName, lastName);
            }
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