using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["stuId"] = "Null";
            localSettings.Values["name"] = "Null";
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(LoginPage));
        }
    }
}
