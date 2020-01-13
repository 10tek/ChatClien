using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;

namespace ClientSocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Socket socket;

        public MainWindow()
        {
            InitializeComponent();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var localIp = IPAddress.Parse("127.0.0.1");
            var port = 8080;
            var endPoint = new IPEndPoint(localIp, port);

            socket.Connect(endPoint);
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            var mes = new Message
            {
                User = userTB.Text,
                Text = textTB.Text
            };

            var jsonMes = JsonConvert.SerializeObject(mes);

            var buffer = Encoding.UTF8.GetBytes(jsonMes);
            ArraySegment<byte> data = new ArraySegment<byte>(buffer);

            await socket.SendAsync(data, SocketFlags.None);

            await GetMessages();
        }

        private async Task GetMessages()
        {
            messageHistory.Content = string.Empty;
            var stringBuilder = new StringBuilder();
            while (socket.Available > 0)
            {
                var buffer = new byte[1024];
                ArraySegment<byte> data = new ArraySegment<byte>(buffer);
                await socket.ReceiveAsync(data, SocketFlags.None);
                stringBuilder.Append(Encoding.UTF8.GetString(buffer));
            }
            var messages = JsonConvert.DeserializeObject<List<Message>>(stringBuilder.ToString());
            foreach(var mes in messages)
            {
                messageHistory.Content += mes.ToString();
            }
            
        }
    }
}
