using GreenThumb.Database;
using GreenThumb.Managers;
using GreenThumb.Models;
using System.Windows;

namespace GreenThumb.Windows
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.ToString().Trim();
            if (!UserManager.ValidateUserRegistration(username, password))
            {
                MessageBox.Show("Not a valid username or password\n\nUsername must be 3 letters long\nPassword must be 5 letters long");
                return;
            }
            using (GreenThumbDbContext context = new())
            {
                GreenThumbRepository<UserModel> userRepo = new(context);
                if (userRepo.GetAll().FirstOrDefault(u => u.Username == username) == null)
                {
                    //User doesnt exist
                    UserModel user = new()
                    {
                        Username = username,
                        Password = password,
                        Garden = new GardenModel() { },
                    };
                    userRepo.Add(user);
                    userRepo.Complete();

                    MessageBox.Show("Registration successfull!");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                    return;
                }
                //User exists
                MessageBox.Show("Username already taken! Please choose another!");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
