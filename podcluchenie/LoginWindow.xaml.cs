using System;
using System.Net;
using System.Windows;

namespace YourNamespace
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string ipAddress = IpAddressTextBox.Text.Trim();
            string username = UsernameTextBox.Text.Trim();

            if (IsValidIp(ipAddress) && !string.IsNullOrEmpty(username))
            {
                // димаааа тут карочэ логика подключения к серверу
                // Если всё успешно, устанавливаю DialogResult в true
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректный IP адрес и имя пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidIp(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }
    }
}
