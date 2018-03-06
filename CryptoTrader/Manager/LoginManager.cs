namespace CryptoTrader.Manager
{
    using System.Collections.Generic;
    using System.Linq;
    using CryptoTrader.Model.DbModel;

    public class LoginManager
    {
        public static bool Login(string email, string password, List<Person> personList)
        {
            bool result = false;
            // Existiert der Login ?
            if (personList.Any(a => a.email.Equals(email, System.StringComparison.CurrentCultureIgnoreCase)))
            {   //User aus der Db holen
                var person = personList.Where(a => a.email.Equals(email, System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
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
    }
}