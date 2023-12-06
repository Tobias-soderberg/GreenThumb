

using GreenThumb.Database;
using GreenThumb.Models;
using System.Windows;

namespace GreenThumb.Managers;

internal static class UserManager
{
    public static UserModel? currentUser;

    internal static bool ValidateLogin(string username, string password)
    {
        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<UserModel> userRepo = new(context);
            UserModel? userToLogin = userRepo.GetAllInclude("Garden.Plants").FirstOrDefault(u => u.Username == username);
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
