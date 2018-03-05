namespace CryptoTrader.Manager
{
    using CryptoTrader.Models.ViewModel;
    using System.IO;
    using System.Web;

    public class UploadImage
    {
        public static string ImageUploadPath(PersonVerificationViewModel vm, int id)
        {
            vm.Path = vm.Upload.FileName;
            if (vm.Upload != null && vm.Upload.ContentLength > 0)
            {
                //Prüft ob der Pfad vorhanden ist
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath("~/Image/ab"));
                //Falls nicht vorhanden Neuerstellen
                if (!exists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Image/ab"));


                var userImagePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Image/ab"), vm.Path);


                vm.Upload.SaveAs(userImagePath);
                return userImagePath;
            }

            return null;
        }
    }
}