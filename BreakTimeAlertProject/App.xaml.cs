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
        private List<MainWindow> windows = new List<MainWindow>();
        AppSettings settings = new AppSettings();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            tray.AppIcon();
            var timer = new System.Timers.Timer(TimeSpan.FromMinutes(settings.Minute).TotalMilliseconds);
            var hideTimer = new System.Timers.Timer(TimeSpan.FromSeconds(settings.Second).TotalMilliseconds); ;

            //Tüm ekranları gezerek screen'e atayarak her bir ekran için yeni bir Mainwindow nesnesi oluşturup konumu boyutu gibi özellikleri verip gizler.
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                MainWindow mainWindow = new MainWindow();
                windows.Add(mainWindow);

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
                            if (hideTimer != null)
                            {
                                hideTimer.Stop();
                            }
                            //Belirtilen süre sonunda var olan pencereleri gezerek hepsini tekrar saklı hale getirir.
                            hideTimer.Elapsed +=
                            delegate (object? s, System.Timers.ElapsedEventArgs a)
                            {
                                foreach (var window in windows)
                                {
                                    window.Dispatcher.Invoke(() =>
                                    {
                                        window.Hide();
                                    });
                                }
                                hideTimer.Stop();
                            };
                            hideTimer.Start();
                            timer.Stop();
                            timer.Start();
                        };
                timer.Start();
            }
        }
    }
}
