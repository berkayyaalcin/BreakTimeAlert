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
using System.Windows.Shapes;

namespace BreakTimeAlertProject
{
    /// <summary>
    /// Interaction logic for AppSettings.xaml
    /// </summary>
    public partial class AppSettings : Window
    {
        public AppSettings()
        {
            InitializeComponent();
            ActiveRadioButton.Checked += ActiveRadioButton_Checked;
            InactiveRadioButton.Checked += InactiveRadioButton_Checked;
            SaveButton.Click += SaveButton_Click;
        }

        
        private void ActiveRadioButton_Checked(object sender, RoutedEventArgs e)
        {
        }
        private void InactiveRadioButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Pencerenin kapatılma özelliği iptal edilerek gizlenmesi olarak değiştirildi.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
