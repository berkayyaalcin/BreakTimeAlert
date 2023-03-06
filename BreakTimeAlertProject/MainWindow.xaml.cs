using BreakTimeAlertProject.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BreakTimeAlertProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppSettings settings = new AppSettings();
        public MainWindow()
        {
            InitializeComponent();
            MainLabel.Content = settings.Labell; //İsim ile belirtilen label content'ine settings üzerinde eriştiğimiz Label değerini yazdırıyoruz.
        }

        /// <summary>
        /// Pencerenin kapatılma özelliği iptal edilerek gizlenmesi olarak değiştirildi.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// Kullanıcı için tanımlanmış tuşlar ile tüm pencereleri kapatma ve ayarları açmak için kısayol oluşturuldu.
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    window.Close();
                }
            }
            if (e.Key == Key.F1) 
            {
                settings.Show();
            }
        }
    }
}
