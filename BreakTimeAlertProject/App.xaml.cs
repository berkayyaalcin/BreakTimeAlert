using BreakTimeAlertProject.Properties;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using IWshRuntimeLibrary;

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
            CheckProcess();
            CreateOrDeleteShortcut();
            base.OnStartup(e);
            tray.AppIcon();
            var timer = new System.Timers.Timer(TimeSpan.FromMinutes(settings.Minute).TotalMilliseconds);
            var hideTimer = new System.Timers.Timer(TimeSpan.FromSeconds(settings.Second).TotalMilliseconds);
            settings.LoadJsonFiletoSettings();

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

                if (settings.ActiveRadioButton.IsChecked == true)
                {
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
                else
                    timer.Stop();
            }
        }

        /// <summary>
        /// Öncelikle mevcut uygulamanın süreci process değişkenine atanır. Sonra uygulamanın adına sahip süreçler alınır ve processes'a atanır.
        /// Foreach ile processes içinde her bir süreç proc değişkenine atanır. 
        /// If koşuluyla mevcut id ile proc.id aynı değil mi ve mevcut isim ile proc.isim aynı mı olarak kontrol edilir. Koşul doğrulanırsa proc ve döngü sonlandırılır.
        /// </summary>
        private void CheckProcess()
        {
            var process = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(process.ProcessName);

            foreach (var proc in processes)
            {
                if (proc.Id != process.Id && proc.MainModule.FileName == process.MainModule.FileName)
                {
                    proc.Kill();
                    break;
                }
            }
        }

        /// <summary>
        /// Oluşturulmuş bu metot OnRadioButton ve OffRadioButton'ları dinler.
        /// Eğer On seçiliyse Startup üzerinde uygulamanın kısayolunu oluşturur. Off seçili ise kısayolu siler.
        /// </summary>
        private void CreateOrDeleteShortcut()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\BreakTimeAlertProject.exe.lnk";

            if (settings.OnRadioButton.IsChecked == true)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.Description = "BreakTimeAlertProject shortcut";
                shortcut.TargetPath = Assembly.GetEntryAssembly().Location.Replace(".dll", ".exe");

                shortcut.Save();
            }
            else if (settings.OffRadioButton.IsChecked == true)
            {

                if (System.IO.File.Exists(shortcutPath))
                {
                    System.IO.File.Delete(shortcutPath);
                }
            }
        }


    }
}
