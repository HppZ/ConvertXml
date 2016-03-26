using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace ConvertXml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        private void Choose_OnClick(object sender, RoutedEventArgs e)
        {
            ShowPreviewText("");
            var r1 = OpenFile();
            var r2 = DeserializeXml(r1);
            if (!string.IsNullOrEmpty(r2))
            {
                ConvertBtn.IsEnabled = true;
                ShowPreviewText(r2);
            }
        }

        /// <summary>
        /// 转换
        /// </summary>
        private void Convert_OnClick(object sender, RoutedEventArgs e)
        {
            var r = Save(PreviewTxt.Text);
            ConvertBtn.IsEnabled = false;
            ShowPreviewText(r ? "succeed!" : "failed!");
        }


        private Stream OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == true)
            {
                return dialog.OpenFile();
            }
            return null;
        }

        private string DeserializeXml(Stream stream)
        {
            XDocument xml = XDocument.Load(stream);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (XElement e in xml.Descendants("Phrase"))
            {
                string eng = e.Element("Eng")?.Value;
                string def = e.Element("Defi")?.Value;
                string all = $"{eng,-40}" + def;
                stringBuilder.AppendLine(all);
            }

            return stringBuilder.ToString();
        }


        private void ShowPreviewText(string str)
        {
            PreviewTxt.Text = str;
        }

        private bool Save(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            var saveDialog = new SaveFileDialog
            {
                FileName = "convertedText",
                DefaultExt = ".text",
                Filter = "Text documents (.txt)|*.txt"
            };
            var result = saveDialog.ShowDialog();
            if (result == true)
            {
                File.WriteAllText(saveDialog.FileName, str);
                return true;
            }

            return false;
        }


    }
}
