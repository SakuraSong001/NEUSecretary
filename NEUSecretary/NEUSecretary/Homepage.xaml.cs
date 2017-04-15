using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using Windows.Storage;
using SQLite.Net;
using System.Text;
using NEUSecretary.Models;
using SQLite.Net.Platform.WinRT;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Homepage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public Homepage()
        {
            // GetWeather();
            this.InitializeComponent();
            this.LeftFlipView.ItemsSource = this.CenterFlipView.ItemsSource = this.RightFlipView.ItemsSource = new ObservableCollection<BitmapImage>()
            {
                new BitmapImage(new System.Uri("ms-appx:///Assets/happy.png",System.UriKind.RelativeOrAbsolute)),
                new BitmapImage(new System.Uri("ms-appx:///Assets/11111.png",System.UriKind.RelativeOrAbsolute)),
                new BitmapImage(new System.Uri("ms-appx:///Assets/091.jpg",System.UriKind.RelativeOrAbsolute))
            };
            this.CenterFlipView.SelectedIndex = 0;
            this.LeftFlipView.SelectedIndex = this.LeftFlipView.Items.Count - 1;
            this.RightFlipView.SelectedIndex = this.CenterFlipView.SelectedIndex + 1;

            DateTime localTime = DateTime.Now;
            WeekdayTextBlock.Text = localTime.DayOfWeek.ToString();
            string greeting;

            if (localTime.Hour < 12)
                greeting = "上午好";
            else if (localTime.Hour == 12)
                greeting = "中午好";
            else if (localTime.Hour < 18)
                greeting = "下午好";
            else
                greeting = "晚上好";
            WelcomeTextBlock.Text = greeting + "，" + localSettings.Values["name"].ToString() + "同学";
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;//每秒触发这个事件，以刷新指针
            timer.Start();
            getTodayClass();
            //GetWeather();

        }

        public async void getTodayClass()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var local = ApplicationData.Current.LocalFolder;
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var id = localSettings.Values["stuId"];
            StorageFile file = await localStorageFolder.GetFileAsync(id + "_class.db");

            SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), file.Path);
            StringBuilder sb = new StringBuilder();
            var list = conn.Table<StudentClass>();
            DateTime localTime = DateTime.Now;
            WeekdayTextBlock.Text = localTime.DayOfWeek.ToString();
            string today = localTime.DayOfWeek.ToString();
            int todayNum = 0;
            switch (today)
            {
                case "Monday":
                    todayNum = 1;
                    break;
                case "Tuesday":
                    todayNum = 2;
                    break;
                case "Wednesday":
                    todayNum = 3;
                    break;
                case "Thursday":
                    todayNum = 4;
                    break;
                case "Friday":
                    todayNum = 5;
                    break;
                case "Saturday":
                    todayNum = 6;
                    break;
                case "Sunday":
                    todayNum = 7;
                    break;
            }
            bool voidMark = true;
            foreach (var item in list)
            {
                if (item.weekday == todayNum)
                {
                    TodayClassTextBlock.Text += item.classinfo;
                    TodayClassTextBlock.Text += "\n";
                    voidMark = false;
                }
            }
            if (voidMark)
            {
                TodayClassTextBlock.Text = "今天木有课~\n背上包来一场说走就走的旅程吧！";
            }
            
        }

        private void Timer_Tick(object sender, object e)
        {
            DateTime localTime = DateTime.Now;
            WeekdayTextBlock.Text = localTime.DayOfWeek.ToString();
            DayTextBlock.Text = localTime.Hour + ":" + localTime.Minute + ":" + localTime.Second;

        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DispatcherTimer timer1 = new DispatcherTimer();
            DispatcherTimer timer2 = new DispatcherTimer();
            DispatcherTimer timer3 = new DispatcherTimer();
            timer1.Interval = timer2.Interval = timer3.Interval = new System.TimeSpan(0, 0, 3);
            timer1.Tick += (sender, args) =>
            {
                this.CenterFlipView.SelectedIndex = this.CenterFlipView.SelectedIndex < this.CenterFlipView.Items.Count - 1 ? ++this.CenterFlipView.SelectedIndex : 0;
            };
            timer2.Tick += (sender, args) =>
            {
                this.LeftFlipView.SelectedIndex = this.LeftFlipView.SelectedIndex < this.LeftFlipView.Items.Count - 1 ? ++this.LeftFlipView.SelectedIndex : 0;
            };
            timer3.Tick += (sender, args) =>
            {
                this.RightFlipView.SelectedIndex = this.RightFlipView.SelectedIndex < this.RightFlipView.Items.Count - 1 ? ++this.RightFlipView.SelectedIndex : 0;
            };
            timer1.Start();
            timer2.Start();
            timer3.Start();
        }

        private async void GetWeather()
        {
            var position = await LocationManager.GetPosition();
            double lon = position.Coordinate.Point.Position.Longitude;
            double lat = position.Coordinate.Point.Position.Latitude;
            RootObject myWeather = await WeatherDataApi.GetWeather(lon, lat);

            String errorCode = myWeather.error_code.ToString();
            if (errorCode == "0")
            {
                Debug.WriteLine("成功");
            }
            else
            {
                Debug.WriteLine("失败");
                Debug.WriteLine(myWeather.error_code.ToString() + ":" + myWeather.reason.ToString());
            }
            WeatherTextBlock.Text = myWeather.result.today.city + " " + myWeather.result.today.temperature + " " + myWeather.result.today.weather;
            WeatherDescribeTextBlock.Text = myWeather.result.today.wind;
            string picPath = "";
            switch (myWeather.result.today.weather)
            {
                case "晴":
                    picPath = "sun-smile-glasses-icon_128.png";
                    break;
                case "小雪":
                    picPath = "snow-snowflakes-icon_128.png";
                    break;
                case "雷阵雨":
                    picPath = "lightning-icon_128.png";
                    break;
                case "小雨":
                    picPath = "rain-icon-weather_128.png";
                    break;
                case "多云":
                    picPath = "cloud-icon-weather_128.png";
                    break;
                default:
                    picPath = "sun-smile-glasses-icon_128.png";
                    break;
            }
            WeatherIconImage.Source = new BitmapImage(new Uri("ms-appx:///Assets//"+picPath, UriKind.Absolute));
        }
    }
}
