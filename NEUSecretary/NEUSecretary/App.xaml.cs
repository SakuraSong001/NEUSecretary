using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

namespace NEUSecretary
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();

                StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///VoiceCommandsFile.xml"));
                // Install the main VCD. Since there's no simple way to test that the VCD has been imported, or that it's your most recent
                // version, it's not unreasonable to do this upon app load.

               /* await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);
                var dialog = new MessageDialog("VCD指令集加载成功");
                await dialog.ShowAsync();*/
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            base.OnActivated(args);
            // 如果程序不是因为语音命令而激活的，就不处理
            if (args.Kind != ActivationKind.VoiceCommand)
                return;

            //将参数转为语音指令事件对象
            var vcargs = (VoiceCommandActivatedEventArgs)args;
            // 分析被识别的命令
            var res = vcargs.Result;
            // 获取被识别的命令的名字
            var cmdName = res.RulePath[0];
            Type navType = null;
            string propertie = null;
            string propertie2 = null;
            //判断用户使用的是哪种语音指令
            switch (cmdName)
            {
                case "OpenMainPage":
                    if (localSettings.Values["stuId"].ToString() == "Null")
                        navType = typeof(LoginPage);
                    else
                        navType = typeof(MainPage);
                    break;
                case "OpenClassPage":
                    if (localSettings.Values["stuId"].ToString() == "Null")
                        navType = typeof(LoginPage);
                    else
                        navType = typeof(Class);
                    break;
                case "OpenRoomPage":
                    if (localSettings.Values["stuId"].ToString() == "Null")
                        navType = typeof(LoginPage);
                    else
                        navType = typeof(Selfstudy);
                    break;
                case "OpenScorePage":
                    if (localSettings.Values["stuId"].ToString() == "Null")
                        navType = typeof(LoginPage);
                    else
                        navType = typeof(Score);
                    break;
                case "OpenLibraryPage":
                    if (localSettings.Values["stuId"].ToString() == "Null")
                        navType = typeof(LoginPage);
                    else
                        navType = typeof(Library);
                    break;
                case "OpenSelfinfoPage":
                    if (localSettings.Values["stuId"].ToString() == "Null")
                        navType = typeof(LoginPage);
                    else
                        navType = typeof(Selfinfo);
                    break;
                case "QueryRoom":
                    //获取语音指令的参数
                    propertie = res.SemanticInterpretation.Properties["Time"][0];
                    localSettings.Values["cortanaUse"] = "true";
                    localSettings.Values["cortanaTime"] = res.SemanticInterpretation.Properties["Time"][0]; ;
                    localSettings.Values["cortanaRoom"] = res.SemanticInterpretation.Properties["Room"][0];

                    //根据 propertie 参数决定跳转到指定界面，这里就不判断了
                    navType = typeof(Selfstudy);
                    break;
            }
            //获取页面引用
            var root = Window.Current.Content as Frame;
            if (root == null)
            {
                root = new Frame();
                Window.Current.Content = root;
            }
            root.Navigate(navType, propertie);

            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }
    }
}
