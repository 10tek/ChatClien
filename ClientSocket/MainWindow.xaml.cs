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

namespace ClientSocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var localIp = IPAddress.Parse("127.0.0.1");
                var port = 8080;
                var endPoint = new IPEndPoint(localIp, port);

                socket.Connect(endPoint);

                var mes =$"{userTB.Text}|userMessage|{textTB.Text}";
                var buffer = Encoding.UTF8.GetBytes(mes);

                socket.Send(buffer);
                socket.Shutdown(SocketShutdown.Both);
            }
        }
    }
}
