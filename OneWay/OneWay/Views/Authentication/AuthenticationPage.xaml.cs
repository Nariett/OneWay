using MahApps.Metro.IconPacks;
using OneWay.Class;
using OneWay.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationPage.xaml
    /// </summary>
    public partial class AuthenticationPage : Page
    {
        DataBase db = new DataBase("OneWayDB.db");
        public event EventHandler<int> AuthenticationCompleted;
        public AuthenticationPage()
        {
            InitializeComponent();
        }

        private void AuthButon(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Login.Text) ||
                string.IsNullOrWhiteSpace(Password.Password))//проверка на заполнение полей
            {
                CustomMessageBox.Show("Уведомление", $"Заполните все поля и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return;
            }
            int id = db.CheckUser(Login.Text, AesEncryption.EncryptString(Password.Password));//шифрование пароля
            if (id != -1)
            {
                AuthenticationCompleted?.Invoke(this, id);
            }
            else
            {
               CustomMessageBox.Show("Уведомление", $"Проверьте логин или пароль.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
            }
        }

        private void RegButtonClick(object sender, RoutedEventArgs e)
        {
            RegistrationPage regPage = new RegistrationPage();
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null && parentWindow is MainWindow mainWindow && mainWindow.fContainer != null)
            {
                mainWindow.fContainer.Content = regPage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ControlTemplate template = button.Template;
                PackIconFontAwesome icon = template.FindName("icon", button) as PackIconFontAwesome;
                if (icon != null)
                {
                    if (Password.Visibility == Visibility.Visible)
                    {
                        PasswordTextBox.Visibility = Visibility.Visible;
                        Password.Visibility = Visibility.Collapsed;
                        PasswordTextBox.Text = Password.Password;
                        icon.Kind = PackIconFontAwesomeKind.EyeSolid;
                    }
                    else
                    {
                        PasswordTextBox.Visibility = Visibility.Collapsed;
                        Password.Visibility = Visibility.Visible;
                        Password.Password = PasswordTextBox.Text;
                        icon.Kind = PackIconFontAwesomeKind.EyeSlashSolid;
                    }
                }
            }
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password.Password = PasswordTextBox.Text;
        }
    }
}
