using BreakTimeAlertProject.Properties;
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
            var timer = new System.Timers.Timer(TimeSpan.FromMinutes(20).TotalMilliseconds);

            //Tüm ekranları gezerek screen'e atayarak her bir ekran için yeni bir Mainwindow nesnesi oluşturup konumu boyutu gibi özellikleri verip gizler.
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                mainWindow.Left = screen.WorkingArea.Left;
                mainWindow.Top = screen.WorkingArea.Top;
                mainWindow.Hide();
                mainWindow.WindowState = WindowState.Normal;

                //Belirtilen süre aralıklarla çalışması üzerine ayarlanmış ve timer çağrısı ile tetiklenecektir.
                timer.Elapsed +=
                        delegate (object? sender, System.Timers.ElapsedEventArgs args)
                        {
                            //Pencereler ekranda gösterilir ve odaklanıp etkinleştirilir. Invoke belirtilen işlevi UI üzerinde çalıştırmak için kullanılır.
                            mainWindow.Dispatcher.Invoke(() =>
                            {
                                mainWindow.Show();
                                mainWindow.WindowState = WindowState.Maximized;
                                mainWindow.Activate();
                                mainWindow.Focus();
                            });
                            timer.Stop();
                            timer.Start();
                        };
                timer.Start();
            }
        }
    }
}
