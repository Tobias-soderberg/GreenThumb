using GreenThumb.Managers;
using GreenThumb.Windows;
using System.Windows;

namespace GreenThumb;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        RegisterWindow registerWindow = new RegisterWindow();
        registerWindow.Show();
        Close();
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Password.ToString().Trim();
        if (UserManager.ValidateLogin(username, password))
        {
            PlantWindow plantWindow = new PlantWindow();
            plantWindow.Show();
            Close();
        }
    }
}
