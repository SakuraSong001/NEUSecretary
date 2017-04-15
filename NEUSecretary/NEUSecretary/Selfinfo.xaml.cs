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
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace NEUSecretary
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Selfinfo : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public Selfinfo()
        {
            this.InitializeComponent();
            MyIdTextBlock.Text = localSettings.Values["stuId"].ToString();
            MyNameTextBlock.Text = localSettings.Values["name"].ToString();
            if (localSettings.Values["userPic"] != null)
            {
                UserHeadPortraitImage.Source = new BitmapImage(new Uri(localSettings.Values["userPic"].ToString(), UriKind.Absolute));
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["stuId"] = "Null";
            localSettings.Values["name"] = "Null";
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(LoginPage));
        }

        private async void ChangePicButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
              PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                var local = ApplicationData.Current.LocalFolder;
                var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
                var newFile = await localStorageFolder.CreateFileAsync(localSettings.Values["stuId"].ToString() + "_pic"+file.FileType, CreationCollisionOption.GenerateUniqueName);
                await FileIO.WriteBufferAsync(newFile, buffer);
                UserHeadPortraitImage.Source= new BitmapImage(new Uri(newFile.Path, UriKind.Absolute));
                localSettings.Values["userPic"] = newFile.Path;
            }
            else
            {
                await new MessageDialog("Operation cancelled." + file.Name).ShowAsync();
            }
        }
    }
}
