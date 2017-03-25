using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void LoggingButton_Click(object sender, RoutedEventArgs e)
        {
            String id = AccountTextBlock.Text;
            String password = PasswordTextBlock.Text;
            String code = IdentityCodeTextBlock.Text;
            String postUrl = "http://uvp.leeeeo.com/auth/";
            
            var local = ApplicationData.Current.LocalFolder;

            //var getResponse = await WebRequest.Create(getUrl).GetResponseAsync();

            using (HttpClient client = new HttpClient())
            {
           
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("id", id),
                    new KeyValuePair<string,string>("password", password),
                    new KeyValuePair<string,string>("code", code),
                };
                var content = new HttpFormUrlEncodedContent(kvp);
                HttpResponseMessage response = await client.PostAsync(new Uri(postUrl), content);

                if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
  
            }

            String getDbUrl = "http://uvp.leeeeo.com/" + id + "_class.db";
        
            HttpClient httpClient = new HttpClient();
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var file = await localStorageFolder.CreateFileAsync(id + "_class.db", CreationCollisionOption.ReplaceExisting);

            List<Byte> allbytes = new List<byte>();
            using (var response = await WebRequest.Create(getDbUrl).GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[4000];
                    int bytesRead = 0;
                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, 4000)) > 0)
                    {
                        allbytes.AddRange(buffer.Take(bytesRead));
                    }
                }
            }
            await FileIO.WriteBytesAsync(file, allbytes.ToArray());

            var dialog = new MessageDialog("登陆成功"+file.Path, "消息提示");

            dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
            dialog.Commands.Add(new UICommand("取消", cmd => { }, commandId: 1));

            //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            //获取返回值
            var result = await dialog.ShowAsync();
        }

        private async void GetIdButton_Click(object sender, RoutedEventArgs e)
        {
            String id = AccountTextBlock.Text;
            String getUrl = "http://uvp.leeeeo.com/auth/" + id;
            String getCheckPicUrl = "http://uvp.leeeeo.com/" + id + "_check.png";
            var local = ApplicationData.Current.LocalFolder;

            HttpClient getClient = new HttpClient();
            var getResponse = await WebRequest.Create(getUrl).GetResponseAsync();

            HttpClient httpClient = new HttpClient();
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var file = await localStorageFolder.CreateFileAsync(id+"_check.png", CreationCollisionOption.ReplaceExisting);

            List<Byte> allbytes = new List<byte>();
            using (var response = await WebRequest.Create(getCheckPicUrl).GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[4000];
                    int bytesRead = 0;
                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, 4000)) > 0)
                    {
                        allbytes.AddRange(buffer.Take(bytesRead));
                    }
                }
            }
            await FileIO.WriteBytesAsync(file, allbytes.ToArray());

            IdentityCodePic.Source = new BitmapImage(new Uri(file.Path));
            GetIdButton.IsEnabled = false;
        }
    }
}
