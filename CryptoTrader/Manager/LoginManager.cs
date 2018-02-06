namespace CryptoTrader.Manager
{
    using System.Linq;
    using CryptoTrader.Models.DbModel;


    public class LoginManager
    {
        public static bool Login( string email, string password )
        {
            bool result = false;
            using( var db = new CryptoTraderEntities() )
            {
                // Existiert der Login ?
                if( db.Person.Any( a => a.email == email ) )
                {   //User aus der Db holen
                    Person dbPerson = db.Person.Where( a => a.email == email ).FirstOrDefault();
                    //Salt an das eingegebenen Pw anhängen
                    password += dbPerson.salt;
                    //PW+salt hashen
                    string eingegPWHash = Hashen.HashBerechnen( password );
                    //Hashes vergleichen
                    if( eingegPWHash == dbPerson.password )
                    {
                        string firstName = dbPerson.firstName;
                        string lastName = dbPerson.lastName;
                        string role = dbPerson.role;
                        Cookies.CreateCookies( email, role, firstName, lastName );
                        result = true;
                    }
                    return result;
                }
            }
            return result;

        }
    }
}