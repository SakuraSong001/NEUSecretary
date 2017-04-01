using System;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using SQLite.Net.Attributes;
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

        public Class()
        {
            this.InitializeComponent();
            InitClass();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TermComboBoxTextBlock == null)
            {
                return;
            }

            var combo = (ComboBox)sender;
            var item = (ComboBoxItem)combo.SelectedItem;
            TermComboBoxTextBlock.Text = item.Content.ToString();
        }

        public async void InitClass()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var local = ApplicationData.Current.LocalFolder;
            var localStorageFolder = await local.CreateFolderAsync("File", CreationCollisionOption.OpenIfExists);
            var id = localSettings.Values["stuId"];
            StorageFile file = await localStorageFolder.GetFileAsync(id+"_class.db");

            SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), file.Path);
            StringBuilder sb = new StringBuilder();
            var list = conn.Table<StudentClass>();
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
