namespace CryptoTrader.Manager
{
    using CryptoTrader.Models.ViewModel;
    using System.IO;
    using System.Web;

    public class UploadImage
    {
        public static string ImageUploadPath(PersonVerificationViewModel vm, int id)
        {
            if (vm.Upload != null && vm.Upload.ContentLength > 0)
            {
                vm.Path = vm.Upload.FileName;

                //Prüft ob der Pfad vorhanden ist
                bool exists = Directory.Exists(HttpContext.Current.Server.MapPath("~/Image/UserImages"));
                //Falls nicht vorhanden Neuerstellen
                if (!exists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Image/UserImages"));


                string userImagePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Image/UserImages"), vm.Path);


                vm.Upload.SaveAs(userImagePath);
                return userImagePath;
            }

            return null;
        }
    }
}