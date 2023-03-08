using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace BreakTimeAlertProject
{
    /// <summary>
    /// Interaction logic for AppSettings.xaml
    /// </summary>
    public partial class AppSettings : Window
    {
        private bool _isActive, _isOn;
        private byte _minute, _second;
        private string? _label;
        public AppSettings()
        {
            InitializeComponent();
            LoadActiveSettings();
            LoadOnSettings();
            ActiveRadioButton.Checked += ActiveRadioButton_Checked;
            InactiveRadioButton.Checked += InactiveRadioButton_Checked;
            OnRadioButton.Checked += OnRadioButton_Checked;
            OffRadioButton.Checked += OffRadioButton_Checked;
            SaveButton.Click += SaveButton_Click;

            Minute = Properties.Settings.Default.Minute; //İlk değerinin daha önce kaydedilen değerle eşlemesi sağlanır.
            MinuteBox.Value = Minute;

            Second = Properties.Settings.Default.Second;
            SecondBox.Value = Second;

            Labell = Properties.Settings.Default.Label;
            TextBox.Text = Labell;

            Properties.Settings.Default.Reload();
            LoadJsonFiletoSettings();
        }

        /// <summary>
        /// Eğer bu buton seçiliyse _isActive'e true değerini atar ve kaydeder.
        /// </summary>
        private void ActiveRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _isActive = true;
            Properties.Settings.Default.Save();
        }
        private void InactiveRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _isActive = false;
            Properties.Settings.Default.Save();
        }

        private void OnRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _isOn = true;
            Properties.Settings.Default.Save();
        }
        private void OffRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _isOn = false;
            Properties.Settings.Default.Save();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Minute = (byte)MinuteBox.Value;
            Properties.Settings.Default.Second = (byte)SecondBox.Value;
            Properties.Settings.Default.Label = (string)TextBox.Text;
            Properties.Settings.Default.isActive = _isActive;
            Properties.Settings.Default.isOn = _isOn;

            Properties.Settings.Default.Save();
            WriteSettingsToJsonFile();
            System.Windows.Forms.Application.Restart();
        }

        /// <summary>
        /// _minute değişkeninin okunmasını ve ayarlanmasını sağlar. set kısmında _minute değişkenine değer atanır ve bu değişkeni saklayıp kaydeder.
        /// </summary>
        public byte Minute
        {
            get { return _minute; }
            set
            {
                _minute = value;
                Properties.Settings.Default.Minute = _minute;
                Properties.Settings.Default.Save();
            }
        }
        public byte Second
        {
            get { return _second; }
            set
            {
                _second = value;
                Properties.Settings.Default.Second = _second;
                Properties.Settings.Default.Save();
            }
        }
        public string Labell
        {
            get { return _label; }
            set
            {
                _label = value;
                Properties.Settings.Default.Label = _label;
                Properties.Settings.Default.Save();
            }
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
        /// _isActive için ayarlardaki varolan değeri atamaya çalışır. Eğer yok veya yüklenemezse otomatik olarak true değeri atar.
        /// _isActive durumuna göre ilgili radiobutton seçilmesi sağlanır.
        /// </summary>
        private void LoadActiveSettings()
        {
            try
            {
                _isActive = Properties.Settings.Default.isActive;
            }
            catch (ConfigurationErrorsException)
            {
                _isActive = true;
            }
            if (_isActive)
            {
                ActiveRadioButton.IsChecked = true;
            }
            else
                InactiveRadioButton.IsChecked = true;
        }
        private void LoadOnSettings()
        {
            try
            {
                _isOn = Properties.Settings.Default.isOn;
            }
            catch (ConfigurationErrorsException)
            {
                _isOn = true;
            }
            if (_isOn)
            {
                OnRadioButton.IsChecked = true;
            }
            else
                OffRadioButton.IsChecked = true;
        }

        /// <summary>
        /// Kaydedilmiş ayarları JSON dosyasından yüklemek için kullanılır. Eğer dosya mevcut değilse Default ayarlar ile oluşturur.
        /// Oluşturulan bu dosya Serialize edilerek JSON dosyasına WriteAllText ile yazdıdırılır.
        /// Settings dosyasından ReadAllText ile okuma yapılıp Deserialize edilir ve ilgili değerlere atanır ve kaydedilir.
        /// </summary>
        public void LoadJsonFiletoSettings()
        {
            string fileName = "settings.json";
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (!File.Exists(path))
            {
                //settings.json dosyası mevcut değil ise verilen değerlerle oluştur.
                var settings = new
                {
                    Label = "Dinlen!",
                    Minute = 20,
                    Second = 20,
                    isActive = true,
                    isOn = false,
                };
                string json = JsonConvert.SerializeObject(settings);
                File.WriteAllText(path, json);
            }

            string jsonContent = File.ReadAllText(path);
            dynamic settingsData = JsonConvert.DeserializeObject(jsonContent);

            Properties.Settings.Default.Label = settingsData.Label;
            Properties.Settings.Default.Minute = settingsData.Minute;
            Properties.Settings.Default.Second = settingsData.Second;
            Properties.Settings.Default.isActive = settingsData.isActive;
            Properties.Settings.Default.isOn = settingsData.isOn;

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Kaydedilen değerleri ilgili etiketlere atayarak Seriliaze edilir.
        /// Sonrasında ise JSON dosyasına WriteAllText ile yazma işlemi gerçekleştirir.
        /// </summary>
        private void WriteSettingsToJsonFile()
        {
            var settings = new
            {
                Label = Properties.Settings.Default.Label,
                Minute = Properties.Settings.Default.Minute,
                Second = Properties.Settings.Default.Second,
                isActive = Properties.Settings.Default.isActive,
                isOn = Properties.Settings.Default.isOn
            };
            string json = JsonConvert.SerializeObject(settings);

            string fileName = "settings.json";
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.WriteAllText(path, json);
        }
        /// <summary>
        /// Pencerenin eğer kullanıcının ESC tuşuna basması durumunda kapanması için kısayol atandı.
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
