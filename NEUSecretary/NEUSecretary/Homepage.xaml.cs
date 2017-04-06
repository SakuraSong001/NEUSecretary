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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Homepage : Page
    {
        public Homepage()
        {
            this.InitializeComponent();
            this.LeftFlipView.ItemsSource = this.CenterFlipView.ItemsSource = this.RightFlipView.ItemsSource = new ObservableCollection<BitmapImage>()
            {
                new BitmapImage(new System.Uri("ms-appx:///Assets/000.jpg",System.UriKind.RelativeOrAbsolute)),
                new BitmapImage(new System.Uri("ms-appx:///Assets/gakki.png",System.UriKind.RelativeOrAbsolute)),
                new BitmapImage(new System.Uri("ms-appx:///Assets/091.jpg",System.UriKind.RelativeOrAbsolute))
            };
            this.CenterFlipView.SelectedIndex = 0;
            this.LeftFlipView.SelectedIndex = this.LeftFlipView.Items.Count - 1;
            this.RightFlipView.SelectedIndex = this.CenterFlipView.SelectedIndex + 1;

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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var position = await LocationManager.GetPosition();
            double lon = position.Coordinate.Point.Position.Longitude;
            double lat = position.Coordinate.Point.Position.Latitude;

            WeatherDescribeTextBlock.Text = lon + "," + lat;

            RootObject myWeather = await WeatherDataApi.GetWeather(lon, lat);

            String errorCode = myWeather.error_code.ToString();
            if(errorCode == "0")
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


        }
    }
}
