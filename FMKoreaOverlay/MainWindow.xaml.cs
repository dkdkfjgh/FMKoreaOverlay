using System;
using System.Collections.Generic;
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

using HtmlAgilityPack;

namespace FMKoreaOverlay
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        FMKoreaCrawler FMKCrawler = new FMKoreaCrawler("baseball");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ReLoader();


        }

        public async Task ReLoader()
        {
            while (true)
            {
                var delayTask = Task.Delay(5000);
                ReLoad();
                await delayTask; // wait until at least 10s elapsed since delayTask created
            }
        }

        private void ReLoad()
        {
            TitleGrid.Children.Clear();
            Color[] colors = new Color[]
{
                Colors.MediumVioletRed,
                Colors.Navy,
                Colors.Black,
                Colors.SaddleBrown,
                Colors.DarkMagenta
};

            
            List<String> Titles = FMKCrawler.PageLoad();
            for (int i = 0; i < 9; i++)
            {
                int colorIndex = Titles[i].GetHashCode() % colors.Count();
                if (colorIndex < 0)
                {
                    colorIndex = colorIndex * -1;
                }
                var label = new Label();
                label.Content = Titles[i];
                label.Height = 40;
                label.FontSize = 24;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.Margin = new Thickness(0, i * 40, 0, 0);
                label.Foreground = new SolidColorBrush(colors[colorIndex]);
                TitleGrid.Children.Add(label);
            }
        }

        private void MWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FMKCrawler.ChangeBoard("humor");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FMKCrawler.ChangeBoard("football_world");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FMKCrawler.ChangeBoard("football_korean");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FMKCrawler.ChangeBoard("baseball");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            FMKCrawler.ChangeBoard("lol");
        }

        private void MWindow_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
    class FMKoreaCrawler
    {
        string url;
        HtmlWeb web;
        HtmlDocument htmlDoc;
        public FMKoreaCrawler(string Board) //생성자
        {
            url = "https://www.fmkorea.com/" + Board;
            web = new HtmlWeb();
        }
        public List<String> PageLoad()
        {
            //Xpath = //*[@id="bd_4330602_0"]/div/table/tbody
            htmlDoc = web.Load(url);
            HtmlNodeCollection hNodeCol = htmlDoc.DocumentNode.SelectNodes("//td[contains(@class, 'title hotdeal_var8')]");
            List<string> TitleLists = new List<string>();
            foreach (var hNode in hNodeCol)
            {
                TitleLists.Add(hNode.InnerText);
            }
            for (int i = 0; i < TitleLists.Count; i++)
            {
                TitleLists[i] = TitleLists[i].Replace("\t", "");
                TitleLists[i] = TitleLists[i].Replace("&quot;", "\"");
                TitleLists[i] = TitleLists[i].Replace("&lt;", "<");
                TitleLists[i] = TitleLists[i].Replace("&gt;", ">");
                TitleLists[i] = TitleLists[i].Replace("&amp;", "&");
                TitleLists[i] = TitleLists[i].Replace("&#035;", "#");
                TitleLists[i] = TitleLists[i].Replace("&#039;", "\'");
            }
            return TitleLists;
        }
        public void ChangeBoard(string Board)
        {
            url = "https://www.fmkorea.com/" + Board;
        }
    }
}
