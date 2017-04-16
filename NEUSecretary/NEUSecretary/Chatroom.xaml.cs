using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Chatroom : Page
    {

        StreamSocket socket;
        bool first = true;
        bool fresh = false;
        public string msg;
        double verticalScrollOffset = 0;

        public Chatroom()
        {
            this.InitializeComponent();
        }
        

        StreamSocketListener listener = null;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (listener != null)
            {
                listener.ConnectionReceived -= OnConnReceived;
                listener.Dispose();
                listener = null;
            }

            listener = new StreamSocketListener();
            listener.ConnectionReceived += OnConnReceived;
            await listener.BindServiceNameAsync("");

            runPort.Text = listener.Information.LocalPort;

            //=========================================================
            var hosts = NetworkInformation.GetHostNames();
            foreach (HostName h in hosts)
            {
                if (h.IPInformation != null && h.Type == HostNameType.Ipv4)
                {
                    runIp.Text = h.DisplayName;
                    break;
                }
            }
        }

        private async void OnConnReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {

            if (first)
            {
                // 获取用于通信的socket
                //StreamSocket socket = args.Socket;
                // 向客户端发送字符串：
                //        你好，我是你外公，我叫服务器。
                DataWriter writer = new DataWriter(args.Socket.OutputStream);

                string content = "刘元兴 joined in.";
                //await new MessageDialog(content,"content").ShowAsync();
                writer.UnicodeEncoding = UnicodeEncoding.Utf8; //注意
                                                               // 计算长度
                uint len = writer.MeasureString(content);
                // 写入长度
                writer.WriteUInt32(len);
                // 再写内容
                writer.WriteString(content);
                // 提交数据
                await writer.StoreAsync();
                first = false;
                // 这个socket不要了，扔掉
                //socket.Dispose();
            }
            DataReader reader = new DataReader(args.Socket.InputStream);
            try
            {
                while (true)
                {
                    uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
                    if (sizeFieldCount != sizeof(uint))
                    {
                        return;
                    }
                    uint stringLength = reader.ReadUInt32();
                    uint actualStringLength = await reader.LoadAsync(stringLength);
                    if (stringLength != actualStringLength)
                    {
                        return;
                    }
                    msg = reader.ReadString(actualStringLength);
                    // 获取用于通信的socket
                    //StreamSocket socket = args.Socket;
                    // 向客户端发送字符串：
                    //        你好，我是你外公，我叫服务器。
                    if (msg != null)
                    {
                        DataWriter writer = new DataWriter(args.Socket.OutputStream);

                        string content = "Leeeeo:" + msg;
                        //await new MessageDialog(content,"content").ShowAsync();
                        writer.UnicodeEncoding = UnicodeEncoding.Utf8; //注意
                                                                       // 计算长度
                        uint len = writer.MeasureString(content);
                        // 写入长度
                        writer.WriteUInt32(len);
                        // 再写内容
                        writer.WriteString(content);
                        // 提交数据
                        await writer.StoreAsync();

                    }


                    /*
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBlock tb = new TextBlock { Text = msg, FontSize = 20 };
                        lbMsg.Children.Add(tb);
                    });
                    */
                }
            }
            catch (Exception exception)
            {
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
            }



        }

        private async void OnConnectToServer(object sender, RoutedEventArgs e)
        {
            if (txtIp.Text.Length == 0 || txtPort.Text.Length == 0)
                return;

            Button b = sender as Button;
            b.IsEnabled = false;

            socket = new StreamSocket();
            //StreamSocket socket = new StreamSocket();
            try
            {
                HostName svname = new HostName(txtIp.Text);

                // 连接
                await socket.ConnectAsync(svname, txtPort.Text);

                // 接收数据
                DataReader reader = new DataReader(socket.InputStream);
                reader.UnicodeEncoding = UnicodeEncoding.Utf8; //注意
                // 长度
                await reader.LoadAsync(sizeof(uint));
                uint len = reader.ReadUInt32();
                // 读内容
                await reader.LoadAsync(len);
                string msg = reader.ReadString(reader.UnconsumedBufferLength);

                displayMsg(msg, "System", "left");
            }
            catch (Exception ex)
            {
  
            }
        }

        private async void btConnectSocket_Click(object sender, RoutedEventArgs e)
        {
            if (socket != null)
            {
                await new MessageDialog("已经连接了Socket").ShowAsync();
                return;
            }
            HostName hostName = null;
            string message = "";
            try
            {
                hostName = new HostName(runIp.Text);
            }
            catch (ArgumentException)
            {
                message = "主机名不可用";
            }
            if (message != "")
            {
                await new MessageDialog(message).ShowAsync();
                return;
            }
            socket = new StreamSocket();
            try
            {
                await socket.ConnectAsync(hostName, runPort.Text);
                await new MessageDialog("连接成功").ShowAsync();
            }
            catch (Exception exception)
            {
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
            }
            /*
            await Task.Run(async () =>
            {
                //创建一个读取器 来读取服务端发送来的数据
                try
                {
                    DataReader reader = new DataReader(socket.InputStream);
                    //接受数据
                    reader.UnicodeEncoding = UnicodeEncoding.Utf8; //注意
                                                                   // 长度
                    await reader.LoadAsync(sizeof(uint));
                    uint len = reader.ReadUInt32();
                    // 读内容
                    await reader.LoadAsync(len);
                    string msg = reader.ReadString(reader.UnconsumedBufferLength);

                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBlock tb = new TextBlock { Text = msg, FontSize = 20, Foreground = new SolidColorBrush(Colors.Black) };
                        lbMsg.Children.Add(tb);
                    });
                }
                catch (Exception exception)
                {
                    if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                    {
                        throw;
                    }
                }
            });
            */
        }

        private async void btSendMsg_Click(object sender, RoutedEventArgs e)
        {
            if (socket == null)
            {
                await new MessageDialog("未连接Socket").ShowAsync();
                return;
            }
            DataWriter writer = new DataWriter(socket.OutputStream);
            string stringToSend = tbMsg.Text;
            writer.WriteUInt32(writer.MeasureString(stringToSend));
            writer.WriteString(stringToSend);

            try
            {
                await writer.StoreAsync();
                displayMsg(tbMsg.Text, "刘元兴", "right");
                getMsg();
                //await new MessageDialog("发送成功").ShowAsync();
            }
            catch (Exception exception)
            {
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
            }
        }

        private async void getMsg()
        {
            DataReader reader = new DataReader(socket.InputStream);
            reader.UnicodeEncoding = UnicodeEncoding.Utf8; //注意
                                                           // 长度
            await reader.LoadAsync(sizeof(uint));
            uint len = reader.ReadUInt32();
            // 读内容
            await reader.LoadAsync(len);
            string msg = reader.ReadString(reader.UnconsumedBufferLength);

            displayMsg(msg, "System", "left");
        }

        private async void btGetMsg_Click(object sender, RoutedEventArgs e)
        {
            DataReader reader = new DataReader(socket.InputStream);
            reader.UnicodeEncoding = UnicodeEncoding.Utf8; //注意
                                                           // 长度
            await reader.LoadAsync(sizeof(uint));
            uint len = reader.ReadUInt32();
            // 读内容
            await reader.LoadAsync(len);
            string msg = reader.ReadString(reader.UnconsumedBufferLength);

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextBlock tb = new TextBlock { Text = msg, FontSize = 20, Foreground = new SolidColorBrush(Colors.Black) };
                lbMsg.Children.Add(tb);
            });
        }

        private void tbMsg_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public async void sendMsg()
        {
            if (socket == null)
            {
                await new MessageDialog("未连接Socket").ShowAsync();
                return;
            }
            DataWriter writer = new DataWriter(socket.OutputStream);
            string stringToSend = tbMsg.Text;
            writer.WriteUInt32(writer.MeasureString(stringToSend));
            writer.WriteString(stringToSend);

            try
            {
                await writer.StoreAsync();
                displayMsg(tbMsg.Text, "刘元兴", "right");
                getMsg();
                //await new MessageDialog("发送成功").ShowAsync();
            }
            catch (Exception exception)
            {
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
            }
        }

        private async void tbMsg_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            /*
            var textBox = sender as TextBox;
            if (textBox != null) TxtMsg = textBox.Text;
            if (key.Key != VirtualKey.Enter) return;
            if (string.IsNullOrEmpty(TxtMsg))
                return;
            await SendTxtMsg();
            */
            if (e.Key == VirtualKey.Enter)
            {
                if(tbMsg.Text=="都在么？")
                {
                    displayMsg(tbMsg.Text, "刘元兴", "right");
                    displayMsg("在", "周浩", "left");
                    displayMsg("嗯~ o(*￣▽￣*)o", "宋寅瑜", "left");
                }
                else if (tbMsg.Text =="以上")
                {
                    displayMsg(tbMsg.Text, "刘元兴", "right");
                    displayMsg("好的", "周浩", "left");
                    displayMsg("好b（￣▽￣）d　", "宋寅瑜", "left");
                }
                else
                {
                    displayMsg(tbMsg.Text, "刘元兴", "right");
                }
            }
                //sendMsg();
        }

        public async void displayMsg(string msg, string user, string left)
        {
           
            string pic;
            if (user == "刘元兴")
            {
                pic = "Leeeeo.png";
            } else if (user=="周浩")
            {
                pic = "Loke.png";
            }
            else if (user=="宋寅瑜")
            {
                pic = "Sakura.png";
            } else
                pic = "default.jpg";
            if (left == "left")
            {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    StackPanel totalPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };
                    StackPanel msgPanel = new StackPanel { Orientation = Orientation.Vertical };
                    TextBlock userName = new TextBlock { Foreground = new SolidColorBrush(Colors.Black), FontSize = 16, HorizontalAlignment = HorizontalAlignment.Left, Text = user };
                    Border msgBackground = new Border { BorderThickness = new Thickness(8, 8, 8, 8), Background = new SolidColorBrush(Color.FromArgb(255, 218, 244, 253)) };
                    TextBlock msgBlock = new TextBlock { Text = msg, MaxWidth = 250, FontSize = 20, Foreground = new SolidColorBrush(Colors.Black), HorizontalAlignment = HorizontalAlignment.Left, TextWrapping = TextWrapping.WrapWholeWords };
                    msgBackground.Child = msgBlock;
                    msgPanel.Children.Add(userName);
                    msgPanel.Children.Add(msgBackground);
                    Ellipse userPic = new Ellipse { Height = 50, Width = 50, Margin = new Thickness(10, 10, 10, 10), Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///Assets//" + pic, UriKind.Absolute)) } };
                    totalPanel.Children.Add(userPic);
                    totalPanel.Children.Add(msgPanel);
                    lbMsg.Children.Add(totalPanel);
                });
            }
            else
            {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    StackPanel totalPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
                    StackPanel msgPanel = new StackPanel { Orientation = Orientation.Vertical };
                    TextBlock userName = new TextBlock { Foreground = new SolidColorBrush(Colors.Black), FontSize = 16, HorizontalAlignment = HorizontalAlignment.Right, Text = user };
                    Border msgBackground = new Border { BorderThickness = new Thickness(8, 8, 8, 8), Background = new SolidColorBrush(Color.FromArgb(255, 243, 243, 243)) };
                    TextBlock msgBlock = new TextBlock { Text = msg, MaxWidth = 250, FontSize = 20, Foreground = new SolidColorBrush(Colors.Black), HorizontalAlignment = HorizontalAlignment.Right, TextWrapping = TextWrapping.WrapWholeWords };
                    msgBackground.Child = msgBlock;
                    msgPanel.Children.Add(userName);
                    msgPanel.Children.Add(msgBackground);
                    Ellipse userPic = new Ellipse { Height = 50, Width = 50, Margin = new Thickness(10, 10, 10, 10), Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///Assets//" + pic, UriKind.Absolute)) } };
                    totalPanel.Children.Add(msgPanel);
                    totalPanel.Children.Add(userPic);
                    lbMsg.Children.Add(totalPanel);
                });
            }
            //Debug.WriteLine(ScrollViewMsg.VerticalOffset);
            //Debug.WriteLine(ScrollViewMsg.ActualHeight + ScrollViewMsg.ScrollableHeight);
           
            ScrollViewMsg.ChangeView(null, ScrollViewMsg.ActualHeight+ScrollViewMsg.ScrollableHeight, null);
            tbMsg.Text = "";
        }
    }
}
