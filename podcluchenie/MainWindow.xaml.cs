using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YourNamespace;

namespace podcluchenie
{
    public partial class MainWindow : Window
    {
        Socket socket;
        ObservableCollection<Chat> chatMessages = new ObservableCollection<Chat>();

        public MainWindow()
        {
            InitializeComponent();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("26.149.91.168", 7777);
            SendUser(new User("Client"));
            ListenMess();
            InitializeComponent();

            LoginWindow loginWindow = new LoginWindow();
            bool? result = loginWindow.ShowDialog();

            if (result == true)
            {
                // если данные введены корректно, можно продолжить работу
                string serverIp = loginWindow.IpAddressTextBox.Text;
                string username = loginWindow.UsernameTextBox.Text;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private async void SendMessenge(string message)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(bytes, SocketFlags.None);
            }
            catch (Exception ex)
            {
                // Обработка ошибок 
            }
        }

        private void But_Click(object sender, RoutedEventArgs e)
        {
            SendMessenge(Mess_Tbx.Text);
        }

        private async void ListenMess()
        {
            try
            {
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    int bytesRead = await socket.ReceiveAsync(bytes, SocketFlags.None);
                    if (bytesRead > 0)
                    {
                        string jsonString = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                        Chat chatMessage = JsonConvert.DeserializeObject<Chat>(jsonString);
                        chatMessages.Add(chatMessage);
                        Main_Lbx.ItemsSource = chatMessages;
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок 
            }
        }

        private async void SendUser(User user)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user);
                var bytes = Encoding.UTF8.GetBytes(json);
                await socket.SendAsync(bytes, SocketFlags.None);
            }
            catch (Exception ex)
            {
                // Обработка ошибок 
            }
        }
        public partial class App : Application
        {
            protected override void OnStartup(StartupEventArgs e)
            {
                base.OnStartup(e);

                LoginWindow loginWindow = new LoginWindow();
                if (loginWindow.ShowDialog() == true)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    this.Shutdown();
                }
            }
        }
    }
}