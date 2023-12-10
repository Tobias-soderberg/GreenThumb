using GreenThumb.Database;
using GreenThumb.Models;
using System.Windows;

namespace GreenThumb.Managers;

internal static class UserManager
{
    public static UserModel? currentUser; //This is always set to the signed-in User while inside the application, but is null while on login/registration windows

    internal static bool ValidateLogin(string username, string password)
    {
        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<UserModel> userRepo = new(context);
            UserModel? userToLogin = userRepo.GetAllInclude("Garden.Plants").FirstOrDefault(u => u.Username == username); //Include both Garden and Plants as those will be used.
            if (userToLogin != null)
            {
                if (userToLogin.Password == password)
                {
                    currentUser = userToLogin;
                    return true;
                }
            }
            MessageBox.Show("Wrong username or password!");
            return false;
        }
    }

    internal static bool ValidateUserRegistration(string username, string password)
    {
        return (username.Length >= 3 && password.Length >= 5);
    }
}
