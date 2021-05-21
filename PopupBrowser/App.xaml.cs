using CommandLine;
using CommandLine.Text;
using SingleInstanceCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PopupBrowser
{
    
    public partial class App : Application, ISingleInstance
    {
        MainWindow mainWin;

        private void App_Startup(object sender, StartupEventArgs e)
        {
            CommandLineOptions options = new CommandLineOptions();
            Parser.Default.ParseArguments<CommandLineOptions>(e.Args)
                .WithParsed<CommandLineOptions>(o =>
                {
                    options = o;
                });

            bool AppMutexOwner = this.InitializeAsFirstInstance($"PopupBrowser_{options.Name}");

            if (!AppMutexOwner)
            {
                Thread.Sleep(500);
                Current.Shutdown();
            }
            else
            {
                mainWin = new MainWindow();
                mainWin.Initialize(options);
            }
                
        }

        public void OnInstanceInvoked(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args[1..])
                .WithParsed<CommandLineOptions>(o =>
                {
                    mainWin.Dispatcher.Invoke(() => mainWin.Initialize(o));
                })
                .WithNotParsed<CommandLineOptions>(missing =>
                {
                    mainWin.Dispatcher.Invoke(() => mainWin.Initialize(new CommandLineOptions()));
                });

        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            SingleInstance.Cleanup();
        }

       

    }
}
