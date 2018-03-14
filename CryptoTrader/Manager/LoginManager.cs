namespace CryptoTrader.Manager
{
    using CryptoTrader.Model.DbModel;
    using System.Collections.Generic;
    using System.Linq;

    public class LoginManager
    {
        public static bool Login(string email, string password, List<Person> personList)
        {
            bool result = false;
            // Existiert der Login ?
            if (personList.Any(a => a.email.Equals(email, System.StringComparison.CurrentCultureIgnoreCase)))
            {   //User aus der Db holen
                Person person = personList.Where(a => a.email.Equals(email, System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (person.active == true)
                {
                    //Salt an das eingegebenen Pw anhängen
                    password += person.salt;
                    //PW+salt hashen
                    string eingegPWHash = Hashen.HashBerechnen(password);
                    //Hashes vergleichen
                    if (eingegPWHash == person.password)
                    {
                        string firstName = person.firstName;
                        string lastName = person.lastName;
                        string role = person.role;

                        Cookies.CreateCookies(email, role, firstName, lastName);
                        result = true;
                    }
                }
                return result;
            }
            return result;
        }

        public static void FromRegisterToLogin(string email, string password, List<Person> personList)
        {
            //User aus der Db holen
            Person dbPerson = personList.Where(a => a.email.Equals(email, System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            string firstName = dbPerson.firstName;
            string lastName = dbPerson.lastName;
            string role = dbPerson.role;

            Cookies.CreateCookies(email, role, firstName, lastName);
        }
    }
}
