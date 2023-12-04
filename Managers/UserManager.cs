

using GreenThumb.Database;
using GreenThumb.Models;

namespace GreenThumb.Managers;

internal static class UserManager
{
    public static int currentUserId;

    internal static bool ValidateLogin(string username, string password)
    {
        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<UserModel> userRepo = new(context);
            UserModel? userToLogin = userRepo.GetAll().FirstOrDefault(u => u.Username == username);
            if (userToLogin != null)
            {
                if (userToLogin.Password == password)
                {
                    currentUserId = userToLogin.UserId;
                    return true;
                }
            }
            return false;
        }
    }

    internal static bool ValidateUserRegistration(string username, string password)
    {
        return (username.Length >= 3 && password.Length >= 5);
    }
}
