using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;
using REghZyFramework.Themes;

namespace PopupBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer AutoCloseTimer;
        CommandLineOptions Options;
        Settings UserSettings;

        public MainWindow() 
        {
            InitializeComponent();
        }

        #region ### Initialization ###

      

        public async void Initialize(CommandLineOptions options) 
        {
            Options = options;
            UserSettings = new Settings(Options.Name);

            if (Options.Style!=StyleMode.Notification && !OSHelper.OSUsesLightTheme)
            {
                ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
                Style = this.FindResource("CustomWindowStyle") as Style;
            }
            #region # Setup Style #

            switch (Options.Style)
            {
                case StyleMode.Window:
                    ResizeMode = ResizeMode.CanResize;
                    break;
                case StyleMode.Fixed:
                    ResizeMode = ResizeMode.NoResize;
                    break;
                case StyleMode.Notification:
                    WindowStyle = WindowStyle.None;
                    ResizeMode = ResizeMode.NoResize;
                    break;
            }

            // ShowAddressBar
            txtSource.Visibility = (Options.ShowAddressBar) ? Visibility.Visible : Visibility.Collapsed;
            
            #endregion

            #region # Setup Size & Position #

            this.Width = Options.SizePoint.X;
            this.Height = Options.SizePoint.Y;

            switch (Options.Position)
            {
                case PositionMode.AtCursor:
                    WindowStartupLocation = WindowStartupLocation.Manual;
                    /* continued in Window_Loaded */
                    break;
                case PositionMode.Center:
                    WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    break;
                case PositionMode.Recall:
                    WindowStartupLocation = WindowStartupLocation.Manual;
                    if (!UserSettings.IsNew)
                    {
                        setSaveWindowPosition(UserSettings.WindowPos, UserSettings.WindowSize);
                    }
                    break;
            }

            #endregion

            Show();

            #region # Launch URL #
            await webView.EnsureCoreWebView2Async();
            
            if (!string.IsNullOrEmpty(options.Url)) 
            {
                if (!options.Url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                    options.Url = $"https://{options.Url}";
                if (Uri.IsWellFormedUriString(options.Url, UriKind.Absolute))
                {
                    webView.CoreWebView2.Navigate(options.Url);
                }
                else
                    webView.CoreWebView2.NavigateToString($"<h1>Malformed Url provided!</h1><b>{options.Url}");
            }
            else
                webView.CoreWebView2.NavigateToString(options.ToHtmlHelpString());
            #endregion
        }

        #endregion
        #region ### Events ###

        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (Options.CloseAfter > 0)
            {
                if (AutoCloseTimer == null)
                {
                    AutoCloseTimer = new Timer(Options.CloseAfter * 1000);
                    AutoCloseTimer.Elapsed += AutoCloseTimer_Elapsed;
                    AutoCloseTimer.Start();
                }
                else {
                    AutoCloseTimer.Stop();
                    AutoCloseTimer.Start();
                }

            }
        }

        private void AutoCloseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => Close());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && Options.EasyClose)
                Close();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            // IsVisible is false if closing by titlebar "x", so only call close if the window lost focus some other way
            if (Options.EasyClose && IsVisible) 
                Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Options.Position == PositionMode.Recall)
            {
                UserSettings.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                UserSettings.WindowPos = new Point(Left, Top);
                UserSettings.WindowSize = new Size(Width, Height);
                UserSettings.Save();
            }
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var p = OSHelper.GetScreenCursor(this);
            p.Offset(Options.OffsetPoint.X, Options.OffsetPoint.Y);
            setSaveWindowPosition(p, new Size(Width, Height));
        }

        

        #endregion

        #region ### Helpers ###

        

        void setSaveWindowPosition(Point pos, Size size)
        {
            this.Width = Math.Min(size.Width, SystemParameters.VirtualScreenWidth);
            this.Height = Math.Min(size.Height, SystemParameters.VirtualScreenHeight);
            this.Left = Math.Min(Math.Max(pos.X, SystemParameters.VirtualScreenLeft), SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - size.Width);
            this.Top = Math.Min(Math.Max(pos.Y, SystemParameters.VirtualScreenTop), SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - size.Height);
        }



        #endregion

        
    }


}
