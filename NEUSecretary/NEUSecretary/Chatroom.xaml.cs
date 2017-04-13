using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

                string content = "Leeeeo joined in.";
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
            runRecMsg.Text = "";

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

                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock tb = new TextBlock { Text = msg, FontSize = 20 };
                    lbMsg.Children.Add(tb);
                });
                //runRecMsg.Text = msg;
                // 释放
                //reader.Dispose();

            }
            catch (Exception ex)
            {
                runRecMsg.Text = ex.Message;
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
                        TextBlock tb = new TextBlock { Text = msg, FontSize = 20 };
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
                //getMsg();
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

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextBlock tb = new TextBlock { Text = msg, FontSize = 20 };
                lbMsg.Children.Add(tb);
            });
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
                TextBlock tb = new TextBlock { Text = msg, FontSize = 20 };
                lbMsg.Children.Add(tb);
            });
        }
    }
}
