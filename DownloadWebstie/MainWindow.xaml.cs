using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadWebstie
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentUrl = WebsiteUrl.Text;
                
            await Task.Run(async () =>
            {           
                var webClient = new WebClient();

                var downloadedString = await webClient.DownloadStringTaskAsync(currentUrl);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    DisplayText.Text = "Sukces";
                });
            });
        }

        private void DoSomeWork()
        {
            Thread.Sleep(2222);
        }
    }
}
