using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using Windows.UI.Core;
using static NEUSecretary.Models.TuringJson;
using System.IO;
using Windows.Data.Json;
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Turing : Page
    {
        public Turing()
        {
            this.InitializeComponent();
        }

        private async void btnSendMsg_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            var local = ApplicationData.Current.LocalFolder;
            string postUrl = "http://www.tuling123.com/openapi/api";
            if(localSettings.Values["stuId"]==null)
            {
                await new MessageDialog("由于您没有登陆不能调戏小秘书", "错误提示").ShowAsync();
                return;
            }
            string id = localSettings.Values["stuId"].ToString();
            string key = "4fd6c3d1d91d4463a6e5dc1af430fc62";
            
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TextBlock tb = new TextBlock { Text = tbMsg.Text, FontSize = 20, HorizontalAlignment = HorizontalAlignment.Right };
                lbMsg.Children.Add(tb);
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
                    string [] stringArr=responseBody.Split('"');
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TextBlock tb = new TextBlock { Text = stringArr[5], FontSize = 20 ,HorizontalAlignment=HorizontalAlignment.Left };
                        lbMsg.Children.Add(tb);
                    });
                    
                    // Jsonb JSONToObject(responseBody);
                    //var json = JsonSerializer.Create();
                    //Rootobject thinw = json.Deserialize<Rootobject>(new JsonTextReader(new StringReader(responseBody)));

                    
                }

            }

        }

        private void ScrollText_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }
    }
}

