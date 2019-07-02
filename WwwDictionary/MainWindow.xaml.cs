using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace WwwDictionary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            int i;
            string text;
            Dictionary<string, int> dict = new Dictionary<string, int>();

            if (tbHostName.Text == "")
            {
                MessageBox.Show("Введите Web-ресурс.");
                return;
            }

            try
            {
                //  контент
                HttpWebRequest HttpReq = WebRequest.CreateHttp(tbHostName.Text);
                HttpReq.Method = WebRequestMethods.Http.Get;
                HttpReq.KeepAlive = true;
                HttpReq.UserAgent = "Internet Explorer";
                HttpReq.Headers.Add(HttpRequestHeader.Translate, "en");
                HttpWebResponse HttpResp = (HttpWebResponse)HttpReq.GetResponse();

                StreamReader HttpPage = new StreamReader(HttpResp.GetResponseStream());

                text = HttpPage.ReadToEnd();
                tbContent.Text = text;
            }
            catch (Exception exc)
            {
                tbContent.Text = exc.Message;
                tbResult.Text = "";
                return;
            }

            // частотный словарь

            // разбираем предложение на отдельные слова
            text = text.Replace(",", " ");
            text = text.Replace(".", " ");
            text = text.Replace("'", " ");
            text = text.Replace("\"", " ");
            text = text.Replace("<", " ");
            text = text.Replace("/>", " ");
            text = text.Replace(":", " ");
            text = text.Replace(";", " ");
            //text = text.Replace("  ", " ");
            //text = text.Replace("   ", " ");

            // обрезать крайние пробелы
            text = text.Trim();

            string[] elems = text.Split(' ');

            for (i = 0; i < elems.Count(); i++)
            {
                if (!dict.ContainsKey(elems[i]))
                {
                    // добавляем новое слово, количество присваиваем = 1
                    dict.Add(elems[i], 1);
                }
                else
                {
                    // увеличиваем количество на 1
                    dict[elems[i]]++;
                }
            }

            // вывод результата на экран
            i = 0;
            Console.WriteLine("\t\tСлово:\t\tКоличество:\n");

            var list = dict.Keys.ToList();
            list.Sort();

            tbResult.Text = "";
            foreach (var key in list)
            {
                i++;
                tbResult.Text += i + ".  " + key + "  -  " + dict[key] + "\n";

            }
        }
    }
}
