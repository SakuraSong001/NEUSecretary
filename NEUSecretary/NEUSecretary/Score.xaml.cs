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
    /// 

    public sealed partial class Score : Page
    {
        public Score()
        {
            this.InitializeComponent();
            cleanScore();
            initScore();
        }

        public void cleanScore()
        {
            score11.Text = "";
            score12.Text = "";
            score13.Text = "";
            score14.Text = "";
            score15.Text = "";
            score16.Text = "";
            score17.Text = "";
            score18.Text = "";
            score21.Text = "";
            score22.Text = "";
            score23.Text = "";
            score24.Text = "";
            score25.Text = "";
            score26.Text = "";
            score27.Text = "";
            score28.Text = "";
        }
        public async void initScore()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            String id = localSettings.Values["stuId"].ToString();
            String password = localSettings.Values["stuPw"].ToString();
            String code = localSettings.Values["code"].ToString();
            String postUrl = "http://uvp.leeeeo.com/score/";

            var local = ApplicationData.Current.LocalFolder;

            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("id", id),
                    new KeyValuePair<string,string>("password", password),
                    new KeyValuePair<string,string>("code", code),
                    new KeyValuePair<string,string>("YearTermNO", "13"),
                    };
                var content = new HttpFormUrlEncodedContent(kvp);
                HttpResponseMessage response = await client.PostAsync(new Uri(postUrl), content);
                if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                }

            }

            String getDbUrl = "http://uvp.leeeeo.com/" + id + "_score.db";
            HttpClient httpClient = new HttpClient();
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var file = await localStorageFolder.CreateFileAsync(id + "_score.db",CreationCollisionOption.GenerateUniqueName);
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
            var list = conn.Table<Scoreinfo>();

            score11.Text = "课程性质";
            score12.Text = "课程号";
            score13.Text = "课程名称";
            score14.Text = "考试类型";
            score15.Text = "学时";
            score16.Text = "学分";
            score17.Text = "成绩类型";
            score18.Text = "总成绩";

            foreach (var item in list)
            {
                score21.Text += item.classPro;
                score22.Text += item.classNo;
                score23.Text += item.className;
                score24.Text += item.classType;
                score25.Text += item.classCost;
                score26.Text += item.classPay;
                score27.Text += item.scoreType;
                score28.Text += item.score;      
            }

            var gpaList = conn.Table<GPA>();
            foreach (var item in gpaList)
            {
                GPATextblock.Text = item.gpa;
            }
        }

        public async void getScore()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            String id = localSettings.Values["stuId"].ToString();
            String password = localSettings.Values["stuPw"].ToString();
            String code = localSettings.Values["code"].ToString();
            String yearTermNo = localSettings.Values["term"].ToString();
            String postUrl = "http://uvp.leeeeo.com/score/";

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

            String getDbUrl = "http://uvp.leeeeo.com/" + id + "_score.db";
            HttpClient httpClient = new HttpClient();
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var file = await localStorageFolder.CreateFileAsync(id + "_score.db", CreationCollisionOption.GenerateUniqueName);
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
            var list = conn.Table<Scoreinfo>();

            score11.Text = "课程性质";
            score12.Text = "课程号";
            score13.Text = "课程名称";
            score14.Text = "考试类型";
            score15.Text = "学时";
            score16.Text = "学分";
            score17.Text = "成绩类型";
            score18.Text = "总成绩";

            foreach (var item in list)
            {
                score21.Text += item.classPro;
                score22.Text += item.classNo;
                score23.Text += item.className;
                score24.Text += item.classType;
                score25.Text += item.classCost;
                score26.Text += item.classPay;
                score27.Text += item.scoreType;
                score28.Text += item.score;
            }

            
            var gpaList = conn.Table<GPA>();
            foreach (var item in gpaList)
            {
                GPATextblock.Text = item.gpa;
            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

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

            cleanScore();
            getScore();
        }
    }
}
