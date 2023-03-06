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

namespace BreakTimeAlertProject
{
    /// <summary>
    /// Interaction logic for AppSettings.xaml
    /// </summary>
    public partial class AppSettings : Window
    {
        private bool _isActive;
        private byte _minute, _second;
        private string? _label;
        public AppSettings()
        {
            InitializeComponent();
            LoadSettings();
            ActiveRadioButton.Checked += ActiveRadioButton_Checked;
            InactiveRadioButton.Checked += InactiveRadioButton_Checked;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Minute = (byte)MinuteBox.Value;
            Properties.Settings.Default.Second = (byte)SecondBox.Value;
            Properties.Settings.Default.Label = (string)TextBox.Text;
            Properties.Settings.Default.isActive = _isActive;

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
        private void LoadSettings()
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
        /// <summary>
        /// Kaydedilmiş ayarları JSON dosyasına yazmayı sağlar. JObject sınıfından JsonData nesnesi yaratılır.
        /// Serialize yapılarak JsonData nesnesi JSON'a dönüştürülerek json'a atılır.
        /// JSON dizesi WriteAlltext kullanılarak path üzerinde belirtilen dosyaya yazılır.
        /// </summary>
        private void WriteSettingsToJsonFile()
        {
            JObject jsonData = new JObject();
            jsonData.Add("Label", Properties.Settings.Default.Label);
            jsonData.Add("Minute", Properties.Settings.Default.Minute);
            jsonData.Add("Second", Properties.Settings.Default.Second);
            jsonData.Add("isActive", Properties.Settings.Default.isActive);

            string json = JsonConvert.SerializeObject(jsonData);

            string fileName = "settings.json";
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Kaydedilmiş ayarları JSON dosyasından yüklemek için kullanılır. ReadAllText ile JSON okunur ve json değişkenine verilir.
        /// Parse yönetim kullanılarak JsonData okunan JSON nesnesine dönüşür ve buradaki ayarları kednisine özellik olarak alır.
        /// Kaydedilmiş olan ayarlar JSON dosyasında bulunan değerler ile güncellenir ve kayıt edilir.
        /// </summary>
        public void LoadJsonFiletoSettings()
        {
            string fileName = "settings.json";
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string json = File.ReadAllText(path);

            JObject jsonData = JObject.Parse(json);

            string label = (string)jsonData["Label"];
            byte minute = (byte)jsonData["Minute"];
            byte second = (byte)jsonData["Second"];
            bool isActive = (bool)jsonData["isActive"];

            Properties.Settings.Default.Label = label;
            Properties.Settings.Default.Minute = minute;
            Properties.Settings.Default.Second = second;
            Properties.Settings.Default.isActive = isActive;

            Properties.Settings.Default.Save();
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
