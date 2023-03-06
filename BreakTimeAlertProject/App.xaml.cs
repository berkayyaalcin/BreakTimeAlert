using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BreakTimeAlertProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        TrayIcon tray = new TrayIcon();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            tray.AppIcon();
        }
    }
}
