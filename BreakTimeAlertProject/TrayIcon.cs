using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace BreakTimeAlertProject
{
    internal class TrayIcon
    {
        private NotifyIcon notifyIcon = new NotifyIcon();
        private ContextMenuStrip trayMenu = new ContextMenuStrip();
        MainWindow mainWindow = new MainWindow();
        private AppSettings settings = new AppSettings();
        public void AppIcon()
        {
            string IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico"); //Uygulamanın çalıştığı dizinin tam yolu getirilir ikon dosyanın ismiyle birleştiriliyor ikonun tam yolu oluşturulmuş olur.
            notifyIcon.Icon = new System.Drawing.Icon(IconPath); //İkon dosyasının verilen tam yolunu kullanarak ikon oluşturuyor.
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipText = "Güncel Ayarlarla Başlatıldı."; notifyIcon.ShowBalloonTip(1); //1 saniye gözükecek bir balon bildirimi oluşturuyor.
            notifyIcon.ContextMenuStrip = trayMenu; //Kullanıcının program özelliklerine ulaşabilmesi amacıyla oluşturuldu.

            trayMenu.Items.Add("Ayarlar", null, OnSettingsClicked);
            trayMenu.Items.Add("Çıkış", null, OnExitClicked);

            //İmleç menü gösterildikten sonra menü üzerinden başka yere kaydırılırsa menünün otomatik gizlenmesi ayarlandı.
            trayMenu.MouseLeave += (sender, args) =>
            {
                trayMenu.Hide();
            };

            notifyIcon.MouseClick += (sender, args) =>
            {
                //Pencere boyutu normal ölçüde olacak şekilde gösteriliyor.
                if (args.Button == MouseButtons.Left)
                {
                    mainWindow.Show();
                    mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    mainWindow.Activate();
                    mainWindow.Show(); 
                }
                //Kullanıcı ikona sağ tık yaptığı zaman gösterilecek olan menünün tıklanmış olduğu yerinde açılması için "Cursor.Position" kullanıldı.
                if (args.Button == MouseButtons.Right)
                {
                    trayMenu.Show(Cursor.Position);
                }
            };
        }
        /// <summary>
        /// Menü üzerinden çıkış yapılırsa notify iconu kaldırır ve uygulamayı tamamen kapatır.
        /// </summary>
        private void OnExitClicked(object? sender, EventArgs e)
        {
            notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
        /// <summary>
        /// Settings penceresinin gösterilmesini sağlar.
        /// </summary>
        private void OnSettingsClicked(object? sender, EventArgs e)
        {
            settings.Show();
        }
    }
}
