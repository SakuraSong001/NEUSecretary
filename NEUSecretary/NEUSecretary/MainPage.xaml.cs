using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using Windows.UI.Core;
using System.IO;
using Windows.Data.Json;
using Windows.UI.Popups;
using Windows.UI;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MyFrame.Navigate(typeof(Homepage));
            TitleTextBlock.Text = "主页";
            Homepage.IsSelected = true;
            _selectedImage = "ms-appx:///Assets/TuringPic1.gif";
            TuringPic.DataContext = this;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyFrame.CanGoBack)
            {
                MyFrame.GoBack();
                Homepage.IsSelected = true;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Homepage.IsSelected)
            {
              
                MyFrame.Navigate(typeof(Homepage));
                TitleTextBlock.Text = "主页";
            }
            else if (Library.IsSelected)
            {
         
                MyFrame.Navigate(typeof(Library));
                TitleTextBlock.Text = "图书馆";
            }
            else if (Class.IsSelected)
            {
        
                MyFrame.Navigate(typeof(Class));
                TitleTextBlock.Text = "课程表";
            }
            else if(Selfstudy.IsSelected)
            {
                
                MyFrame.Navigate(typeof(Selfstudy));
                TitleTextBlock.Text = "上自习";
            }
            else if(Score.IsSelected)
            {
                
                MyFrame.Navigate(typeof(Score));
                TitleTextBlock.Text = "查成绩";
            }
            else if(Selfinfo.IsSelected)
            {
                
                MyFrame.Navigate(typeof(Selfinfo));
                TitleTextBlock.Text = "个人信息";
            }
            else if (ChatRoom.IsSelected)
            {

                MyFrame.Navigate(typeof(Chatroom));
                TitleTextBlock.Text = "聊天室";
            }
        }

        private void LoggedButton_Click(object sender, RoutedEventArgs e)
        {
            
            MyFrame.Navigate(typeof(LoginPage));
            TitleTextBlock.Text = "登录";
        }

        private async void TuringBtn_Click(object sender, RoutedEventArgs e)
        {
            TuringMsgPanel.Children.Clear();
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextBlock tb = new TextBlock { Text = "我能为你做点什么？", FontSize = 20, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 100, 180)), Width = 280, TextWrapping = TextWrapping.Wrap };
                TuringMsgPanel.Children.Add(tb);
            });
            TuringScrollView.ChangeView(null, TuringScrollView.ActualHeight+ TuringScrollView.ScrollableHeight, null);
            
        }

        private string _selectedImage;
        public string SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                OnPropertyChanged("SelectedImage");
            }
            
        }

        private void OnPropertyChanged(string v)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void tbMsg_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                sendMsgToServer(tbMsg.Text);
            }
        }

        public async void sendMsgToServer(string msg)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            var local = ApplicationData.Current.LocalFolder;
            string postUrl = "http://www.tuling123.com/openapi/api";
            if (localSettings.Values["stuId"] == null)
            {
                await new MessageDialog("由于您没有登陆不能调戏小秘书", "错误提示").ShowAsync();
                return;
            }
            string id = localSettings.Values["stuId"].ToString();
            string key = "4fd6c3d1d91d4463a6e5dc1af430fc62";


            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextBlock tb = new TextBlock { Text = tbMsg.Text, FontSize = 16, HorizontalAlignment = HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black) ,Width=280,FlowDirection=FlowDirection.RightToLeft,TextWrapping=TextWrapping.Wrap };
                TuringMsgPanel.Children.Add(tb);
            });


            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("userid", id),
                    new KeyValuePair<string,string>("info", tbMsg.Text),
                    new KeyValuePair<string,string>("key", key),
                };
                var content = new HttpFormUrlEncodedContent(kvp);
                HttpResponseMessage response = await client.PostAsync(new Uri(postUrl), content);
                if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    string[] stringArr = responseBody.Split('"');

                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBlock tb = new TextBlock { Text = stringArr[5], FontSize = 20, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 100, 180)) , Width = 280, TextWrapping = TextWrapping.Wrap };
                        TuringMsgPanel.Children.Add(tb);
                    });

                    // Jsonb JSONToObject(responseBody);
                    //var json = JsonSerializer.Create();
                    //Rootobject thinw = json.Deserialize<Rootobject>(new JsonTextReader(new StringReader(responseBody)));


                }

            }
            TuringScrollView.ChangeView(null, TuringScrollView.ActualHeight + TuringScrollView.ScrollableHeight, null);
            tbMsg.Text = "";
        }
    }
}
