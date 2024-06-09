using MahApps.Metro.IconPacks;
using OneWay.Class;
using OneWay.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OneWay.Pages
{
    public partial class RegistrationPage : Page
    {
        DataBase db = new DataBase("OneWayDB.db");
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) ||
                string.IsNullOrWhiteSpace(Login.Text) ||
                string.IsNullOrWhiteSpace(Password.Password)) // проверка заполения полей 
            {
                CustomMessageBox.Show("Уведомление", $"Заполните все поля и повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Error);
                return;
            }
            User user = new User
            {
                FirstName = Name.Text,
                Login = Login.Text,
                Password = AesEncryption.EncryptString(Password.Password),
                RegistrationDate = DateTime.Now.ToString("dd.MM.yyyy")
            };
            if(db.InsertUser(user))
            {
                CustomMessageBox.Show("Уведомление", $"Пользователь добавлен.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
                NavigationService?.GoBack();
            }
            else
            {
                CustomMessageBox.Show("Уведомление", $"Пользователь не добавлен. Возможно данный логин уже занят. Повторите попытку.", MessageBoxButton.OK, CustomMessageBox.MessageBoxImage.Information);
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

        private void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Password.Password = PasswordTextBox.Text;
        }
    }
}
