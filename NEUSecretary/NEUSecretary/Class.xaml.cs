using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Text;
using NEUSecretary.Models;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Class : Page
    {
        public ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public Class()
        {
            this.InitializeComponent();
            InitClass();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TermComboBoxTextBlock == null)
            {
                return;
            }

            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;
            if (item.Content.ToString() == "2016-2017学年第一学期")
            {
                localSettings.Values["term"] = 13;
            }
            else if (item.Content.ToString() == "2016-2017学年第二学期")
            {
                localSettings.Values["term"] = 14;
            }
            else if (item.Content.ToString() == "2015-2016学年第二学期")
            {
                localSettings.Values["term"] = 12;
            }
            else if (item.Content.ToString() == "2015-2016学年第一学期")
            {
                localSettings.Values["term"] = 11;
            }
            else if (item.Content.ToString() == "2014-2015学年第二学期")
            {
                localSettings.Values["term"] = 10;
            }
            else if (item.Content.ToString() == "2014-2015学年第一学期")
            {
                localSettings.Values["term"] = 9;
            }

            TermComboBoxTextBlock.Text = localSettings.Values["term"].ToString();
            getClass();
        }
        public async void getClass()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            String id = localSettings.Values["stuId"].ToString();
            String password = localSettings.Values["stuPw"].ToString();
            String code = localSettings.Values["code"].ToString();
            String yearTermNo = localSettings.Values["term"].ToString();
            String postUrl = "http://uvp.leeeeo.com/auth/";

            var local = ApplicationData.Current.LocalFolder;

            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("id", id),
                    new KeyValuePair<string,string>("password", password),
                    new KeyValuePair<string,string>("code", code),
                    new KeyValuePair<string,string>("YearTermNO", yearTermNo),
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
            var file = await localStorageFolder.CreateFileAsync(id + "_class.db", CreationCollisionOption.GenerateUniqueName);
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

            SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), file.Path);
            StringBuilder sb = new StringBuilder();
            var list = conn.Table<StudentClass>();
            class76.Text = "";
            class75.Text = "";
            class74.Text = "";
            class73.Text = "";
            class72.Text = "";
            class71.Text = "";
            class66.Text = "";
            class65.Text = "";
            class64.Text = "";
            class63.Text = "";
            class62.Text = "";
            class61.Text = "";
            class56.Text = "";
            class55.Text = "";
            class54.Text = "";
            class53.Text = "";
            class52.Text = "";
            class51.Text = "";
            class46.Text = "";
            class45.Text = "";
            class44.Text = "";
            class43.Text = "";
            class42.Text = "";
            class41.Text = "";
            class36.Text = "";
            class35.Text = "";
            class34.Text = "";
            class33.Text = "";
            class32.Text = "";
            class31.Text = "";
            class26.Text = "";
            class25.Text = "";
            class24.Text = "";
            class23.Text = "";
            class22.Text = "";
            class21.Text = "";
            class16.Text = "";
            class15.Text = "";
            class14.Text = "";
            class13.Text = "";
            class12.Text = "";
            class11.Text = "";

            foreach (var item in list)
            {
                switch (item.weekday)
                {
                    case 7:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class76.Text = item.classinfo;
                                    break;
                                case 5:
                                    class75.Text = item.classinfo;
                                    break;
                                case 4:
                                    class74.Text = item.classinfo;
                                    break;
                                case 3:
                                    class73.Text = item.classinfo;
                                    break;
                                case 2:
                                    class72.Text = item.classinfo;
                                    break;
                                case 1:
                                    class71.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }

                    case 6:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class66.Text = item.classinfo;
                                    break;
                                case 5:
                                    class65.Text = item.classinfo;
                                    break;
                                case 4:
                                    class64.Text = item.classinfo;
                                    break;
                                case 3:
                                    class63.Text = item.classinfo;
                                    break;
                                case 2:
                                    class62.Text = item.classinfo;
                                    break;
                                case 1:
                                    class61.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 5:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class56.Text = item.classinfo;
                                    break;
                                case 5:
                                    class56.Text = item.classinfo;
                                    break;
                                case 4:
                                    class54.Text = item.classinfo;
                                    break;
                                case 3:
                                    class53.Text = item.classinfo;
                                    break;
                                case 2:
                                    class52.Text = item.classinfo;
                                    break;
                                case 1:
                                    class51.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 4:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class46.Text = item.classinfo;
                                    break;
                                case 5:
                                    class45.Text = item.classinfo;
                                    break;
                                case 4:
                                    class44.Text = item.classinfo;
                                    break;
                                case 3:
                                    class43.Text = item.classinfo;
                                    break;
                                case 2:
                                    class42.Text = item.classinfo;
                                    break;
                                case 1:
                                    class41.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 3:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class36.Text = item.classinfo;
                                    break;
                                case 5:
                                    class35.Text = item.classinfo;
                                    break;
                                case 4:
                                    class34.Text = item.classinfo;
                                    break;
                                case 3:
                                    class33.Text = item.classinfo;
                                    break;
                                case 2:
                                    class32.Text = item.classinfo;
                                    break;
                                case 1:
                                    class31.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class26.Text = item.classinfo;
                                    break;
                                case 5:
                                    class25.Text = item.classinfo;
                                    break;
                                case 4:
                                    class24.Text = item.classinfo;
                                    break;
                                case 3:
                                    class23.Text = item.classinfo;
                                    break;
                                case 2:
                                    class22.Text = item.classinfo;
                                    break;
                                case 1:
                                    class21.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 1:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class16.Text = item.classinfo;
                                    break;
                                case 5:
                                    class15.Text = item.classinfo;
                                    break;
                                case 4:
                                    class14.Text = item.classinfo;
                                    break;
                                case 3:
                                    class13.Text = item.classinfo;
                                    break;
                                case 2:
                                    class12.Text = item.classinfo;
                                    break;
                                case 1:
                                    class11.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }

                }


                //sb.AppendLine($"{item.id} {item.weekday} {item.time} {item.classinfo}");
            }
            // await new MessageDialog(sb.ToString() + file.Path).ShowAsync();



        }
        public async void InitClass()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var local = ApplicationData.Current.LocalFolder;
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var id = localSettings.Values["stuId"];
            StorageFile file = await localStorageFolder.GetFileAsync(id + "_class.db");

            SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), file.Path);
            StringBuilder sb = new StringBuilder();
            var list = conn.Table<StudentClass>();

            class76.Text = "";
            class75.Text = "";
            class74.Text = "";
            class73.Text = "";
            class72.Text = "";
            class71.Text = "";
            class66.Text = "";
            class65.Text = "";
            class64.Text = "";
            class63.Text = "";
            class62.Text = "";
            class61.Text = "";
            class56.Text = "";
            class55.Text = "";
            class54.Text = "";
            class53.Text = "";
            class52.Text = "";
            class51.Text = "";
            class46.Text = "";
            class45.Text = "";
            class44.Text = "";
            class43.Text = "";
            class42.Text = "";
            class41.Text = "";
            class36.Text = "";
            class35.Text = "";
            class34.Text = "";
            class33.Text = "";
            class32.Text = "";
            class31.Text = "";
            class26.Text = "";
            class25.Text = "";
            class24.Text = "";
            class23.Text = "";
            class22.Text = "";
            class21.Text = "";
            class16.Text = "";
            class15.Text = "";
            class14.Text = "";
            class13.Text = "";
            class12.Text = "";
            class11.Text = "";

            foreach (var item in list)
            {
                switch (item.weekday)
                {
                    case 7:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class76.Text = item.classinfo;
                                    break;
                                case 5:
                                    class75.Text = item.classinfo;
                                    break;
                                case 4:
                                    class74.Text = item.classinfo;
                                    break;
                                case 3:
                                    class73.Text = item.classinfo;
                                    break;
                                case 2:
                                    class72.Text = item.classinfo;
                                    break;
                                case 1:
                                    class71.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }

                    case 6:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class66.Text = item.classinfo;
                                    break;
                                case 5:
                                    class65.Text = item.classinfo;
                                    break;
                                case 4:
                                    class64.Text = item.classinfo;
                                    break;
                                case 3:
                                    class63.Text = item.classinfo;
                                    break;
                                case 2:
                                    class62.Text = item.classinfo;
                                    break;
                                case 1:
                                    class61.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 5:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class56.Text = item.classinfo;
                                    break;
                                case 5:
                                    class56.Text = item.classinfo;
                                    break;
                                case 4:
                                    class54.Text = item.classinfo;
                                    break;
                                case 3:
                                    class53.Text = item.classinfo;
                                    break;
                                case 2:
                                    class52.Text = item.classinfo;
                                    break;
                                case 1:
                                    class51.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 4:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class46.Text = item.classinfo;
                                    break;
                                case 5:
                                    class45.Text = item.classinfo;
                                    break;
                                case 4:
                                    class44.Text = item.classinfo;
                                    break;
                                case 3:
                                    class43.Text = item.classinfo;
                                    break;
                                case 2:
                                    class42.Text = item.classinfo;
                                    break;
                                case 1:
                                    class41.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 3:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class36.Text = item.classinfo;
                                    break;
                                case 5:
                                    class35.Text = item.classinfo;
                                    break;
                                case 4:
                                    class34.Text = item.classinfo;
                                    break;
                                case 3:
                                    class33.Text = item.classinfo;
                                    break;
                                case 2:
                                    class32.Text = item.classinfo;
                                    break;
                                case 1:
                                    class31.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class26.Text = item.classinfo;
                                    break;
                                case 5:
                                    class25.Text = item.classinfo;
                                    break;
                                case 4:
                                    class24.Text = item.classinfo;
                                    break;
                                case 3:
                                    class23.Text = item.classinfo;
                                    break;
                                case 2:
                                    class22.Text = item.classinfo;
                                    break;
                                case 1:
                                    class21.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }
                    case 1:
                        {
                            switch (item.time)
                            {
                                case 6:
                                    class16.Text = item.classinfo;
                                    break;
                                case 5:
                                    class15.Text = item.classinfo;
                                    break;
                                case 4:
                                    class14.Text = item.classinfo;
                                    break;
                                case 3:
                                    class13.Text = item.classinfo;
                                    break;
                                case 2:
                                    class12.Text = item.classinfo;
                                    break;
                                case 1:
                                    class11.Text = item.classinfo;
                                    break;
                            }
                            break;
                        }

                }


                //sb.AppendLine($"{item.id} {item.weekday} {item.time} {item.classinfo}");
            }
            // await new MessageDialog(sb.ToString() + file.Path).ShowAsync();
        }
    }
}
