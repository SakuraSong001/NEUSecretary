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
using System.Collections.ObjectModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍


namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public class SelfStudyClassroom
    {
        public string roomid { get; set; }
        public string roomtype { get; set; }

        public SelfStudyClassroom(string id,string type)
        {
            roomid = id;
            roomtype = type;
        }
    }

    public sealed partial class Selfstudy : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public Selfstudy()
        {
            this.InitializeComponent();
            if (localSettings.Values["cortanaUse"] !=null)
                if (localSettings.Values["cortanaUse"].ToString() == "true")
            {
                QueryRoom();
            }
        }

        public async void QueryRoom()
        {
            DateTime localTime = DateTime.Now;
            localSettings.Values["roomWeekday"] = (int)localTime.DayOfWeek;
            localSettings.Values["roomTerm"] = 14;

            if (localSettings.Values["cortanaTime"].ToString() == "1-2")
            {
                localSettings.Values["roomStartSection"] = 1;
                localSettings.Values["roomEndSection"] = 2;
            }
            else if (localSettings.Values["cortanaTime"].ToString() == "3-4")
            {
                localSettings.Values["roomStartSection"] = 3;
                localSettings.Values["roomEndSection"] = 4;
            }
            else if (localSettings.Values["cortanaTime"].ToString() == "5-6")
            {
                localSettings.Values["roomStartSection"] = 5;
                localSettings.Values["roomEndSection"] = 6;
            }
            else if (localSettings.Values["cortanaTime"].ToString() == "7-8")
            {
                localSettings.Values["roomStartSection"] = 7;
                localSettings.Values["roomEndSection"] = 8;
            }
            else if (localSettings.Values["cortanaTime"].ToString() == "9-10")
            {
                localSettings.Values["roomStartSection"] = 9;
                localSettings.Values["roomEndSection"] = 10;
            }
            else if (localSettings.Values["cortanaTime"].ToString() == "11-12")
            {
                localSettings.Values["roomStartSection"] = 11;
                localSettings.Values["roomEndSection"] = 12;
            }

            if (localSettings.Values["cortanaRoom"].ToString() == "大成教学楼")
            {
                localSettings.Values["roomId"] = "0000";
            }
            else if (localSettings.Values["cortanaRoom"].ToString() == "逸夫楼")
            {
                localSettings.Values["roomId"] = "0001";
            }
            else if (localSettings.Values["cortanaRoom"].ToString() == "机电馆")
            {
                localSettings.Values["roomId"] = "0003";
            }
            else if (localSettings.Values["cortanaRoom"].ToString() == "信息学馆")
            {
                localSettings.Values["roomId"] = "0104";
            }
            else if (localSettings.Values["cortanaRoom"].ToString() == "文管学馆")
            {
                localSettings.Values["roomId"] = "0101";
            }
            else if (localSettings.Values["cortanaRoom"].ToString() == "建筑学馆")
            {
                localSettings.Values["roomId"] = "0102";
            }
            else if (localSettings.Values["cortanaRoom"].ToString() == "生命学馆")
            {
                localSettings.Values["roomId"] = "0103";
            }


            String id = localSettings.Values["stuId"].ToString();
            String password = localSettings.Values["stuPw"].ToString();
            String code = localSettings.Values["code"].ToString();
            String yearTermNo = localSettings.Values["roomTerm"].ToString();
            String weekDay = localSettings.Values["roomWeekday"].ToString();
            String startSection = localSettings.Values["roomStartSection"].ToString();
            String endSection = localSettings.Values["roomEndSection"].ToString();
            String resultWeek = "7";
            String storyNo = localSettings.Values["roomId"].ToString();

            String postUrl = "http://uvp.leeeeo.com/room/";

            var local = ApplicationData.Current.LocalFolder;

            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("id", id),
                    new KeyValuePair<string,string>("password", password),
                    new KeyValuePair<string,string>("code", code),
                    new KeyValuePair<string,string>("YearTermNO", yearTermNo),
                    new KeyValuePair<string,string>("WeekdayID", weekDay),
                    new KeyValuePair<string,string>("StartSection", startSection),
                    new KeyValuePair<string,string>("EndSection", endSection),
                    new KeyValuePair<string,string>("ResultWeeks", resultWeek),
                    new KeyValuePair<string,string>("STORYNO", storyNo),
                    };
                var content = new HttpFormUrlEncodedContent(kvp);
                HttpResponseMessage response = await client.PostAsync(new Uri(postUrl), content);
                if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                }

            }

            String getDbUrl = "http://uvp.leeeeo.com/" + id + "_room.db";
            HttpClient httpClient = new HttpClient();
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var file = await localStorageFolder.CreateFileAsync(id + "_room.db", CreationCollisionOption.GenerateUniqueName);
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
            var list = conn.Table<ROOM>();

            
            List<SelfStudyClassroom> Classroom = new List<SelfStudyClassroom>();
            foreach (var item in list)
            {
                Classroom.Add(new SelfStudyClassroom( item.classroom, item.roominfo ));

            }
            SstudyGridView.ItemsSource = Classroom;

            var dialog = new MessageDialog("查询到信息");
            await dialog.ShowAsync();
            localSettings.Values["cortanaUse"] = "false";

        }
       
        private void WeekdayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;
            if (item.Content.ToString() == "星期一")
            {
                localSettings.Values["roomWeekday"] = 1;
            }
            else if (item.Content.ToString() == "星期二")
            {
                localSettings.Values["roomWeekday"] = 2;
            }
            else if (item.Content.ToString() == "星期三")
            {
                localSettings.Values["roomWeekday"] = 3;
            }
            else if (item.Content.ToString() == "星期四")
            {
                localSettings.Values["roomWeekday"] = 4;
            }
            else if (item.Content.ToString() == "星期五")
            {
                localSettings.Values["roomWeekday"] = 5;
            }
            else if (item.Content.ToString() == "星期六")
            {
                localSettings.Values["roomWeekday"] = 6;
            }
            else if (item.Content.ToString() == "星期日")
            {
                localSettings.Values["roomWeekday"] = 7;
            }
        }

        private void TimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;
            if (item.Content.ToString() == "1-2")
            {
                localSettings.Values["roomStartSection"] = 1;
                localSettings.Values["roomEndSection"] = 2;
            }
            else if (item.Content.ToString() == "3-4")
            {
                localSettings.Values["roomStartSection"] = 3;
                localSettings.Values["roomEndSection"] = 4;
            }
            else if (item.Content.ToString() == "5-6")
            {
                localSettings.Values["roomStartSection"] = 5;
                localSettings.Values["roomEndSection"] = 6;
            }
            else if (item.Content.ToString() == "7-8")
            {
                localSettings.Values["roomStartSection"] = 7;
                localSettings.Values["roomEndSection"] = 8;
            }
            else if (item.Content.ToString() == "9-10")
            {
                localSettings.Values["roomStartSection"] = 9;
                localSettings.Values["roomEndSection"] = 10;
            }
            else if (item.Content.ToString() == "11-12")
            {
                localSettings.Values["roomStartSection"] = 11;
                localSettings.Values["roomEndSection"] = 12;
            }
        }
        
        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            String id = localSettings.Values["stuId"].ToString();
            String password = localSettings.Values["stuPw"].ToString();
            String code = localSettings.Values["code"].ToString();
            String yearTermNo = localSettings.Values["roomTerm"].ToString();
            String weekDay = localSettings.Values["roomWeekday"].ToString();
            String startSection = localSettings.Values["roomStartSection"].ToString();
            String endSection = localSettings.Values["roomEndSection"].ToString();
            String resultWeek = ResultWeek.Text;
            String storyNo = localSettings.Values["roomId"].ToString();

            String postUrl = "http://uvp.leeeeo.com/room/";
            SelfStudyRelativePanel.Height = 200;

            var local = ApplicationData.Current.LocalFolder;
            

            using (HttpClient client = new HttpClient())
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("id", id),
                    new KeyValuePair<string,string>("password", password),
                    new KeyValuePair<string,string>("code", code),
                    new KeyValuePair<string,string>("YearTermNO", yearTermNo),
                    new KeyValuePair<string,string>("WeekdayID", weekDay),
                    new KeyValuePair<string,string>("StartSection", startSection),
                    new KeyValuePair<string,string>("EndSection", endSection),
                    new KeyValuePair<string,string>("ResultWeeks", resultWeek),
                    new KeyValuePair<string,string>("STORYNO", storyNo),
                    };
                var content = new HttpFormUrlEncodedContent(kvp);
                HttpResponseMessage response = new HttpResponseMessage();
                try {
                    response = await client.PostAsync(new Uri(postUrl), content);
                }
                catch (Exception exception)
                {
                    new MessageDialog(exception.Message, "出现异常");
                    return;
                }
                if (response.EnsureSuccessStatusCode().StatusCode.ToString().ToLower() == "ok")
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                }

            }

            String getDbUrl = "http://uvp.leeeeo.com/" + id + "_room.db";
            HttpClient httpClient = new HttpClient();
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var file = await localStorageFolder.CreateFileAsync(id + "_room.db", CreationCollisionOption.GenerateUniqueName);
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
            var list = conn.Table<ROOM>();

            List<SelfStudyClassroom> Classroom = new List<SelfStudyClassroom> ();
            foreach (var item in list)
            {
                Classroom.Add(new SelfStudyClassroom(item.classroom,item.roominfo));
                
            }
            SstudyGridView.ItemsSource = Classroom;
        }

   
        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;
            if (item.Content.ToString() == "大成教学楼")
            {
                localSettings.Values["roomId"] = "0000";
            }
            else if (item.Content.ToString() == "逸夫楼")
            {
                localSettings.Values["roomId"] = "0001";
            }
            else if (item.Content.ToString() == "机电馆")
            {
                localSettings.Values["roomId"] = "0003";
            }
            else if (item.Content.ToString() == "信息学馆")
            {
                localSettings.Values["roomId"] = "0104";
            }
            else if (item.Content.ToString() == "文管学馆")
            {
                localSettings.Values["roomId"] = "0101";
            }
            else if (item.Content.ToString() == "建筑学馆")
            {
                localSettings.Values["roomId"] = "0102";
            }
            else if (item.Content.ToString() == "生命学馆")
            {
                localSettings.Values["roomId"] = "0103";
            }
        }

        private void TermChooseButton_Click(object sender, RoutedEventArgs e)
        {
            TermBox.Visibility = new Visibility();
            TermBox.Visibility = Visibility.Visible;   
        }

        private void TermBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;
            TermTextBlock.Text = item.Content.ToString();
            TermBox.Visibility = new Visibility();
            TermBox.Visibility = Visibility.Collapsed;
            if (item.Content.ToString() == "2016-2017学年第一学期")
            {                
                localSettings.Values["roomTerm"] = 13;
            }
            else if (item.Content.ToString() == "2016-2017学年第二学期")
            {
                localSettings.Values["roomTerm"] = 14;
            }
            else if (item.Content.ToString() == "2015-2016学年第二学期")
            {
                localSettings.Values["roomTerm"] = 12;
            }
            else if (item.Content.ToString() == "2015-2016学年第一学期")
            {
                localSettings.Values["roomTerm"] = 11;
            }
            else if (item.Content.ToString() == "2014-2015学年第二学期")
            {
                localSettings.Values["roomTerm"] = 10;
            }
            else if (item.Content.ToString() == "2014-2015学年第一学期")
            {
                localSettings.Values["roomTerm"] = 9;
            }
        }

    
    }
}
