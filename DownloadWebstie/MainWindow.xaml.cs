using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace DownloadWebstie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// String that is downloaded from web
        /// </summary>
        public string DownloadedString { get; set; }
        public FileDownloadData FileDownloadData { get; set; } = new FileDownloadData();
        public event Action<string> StringDownloaded = (x) => { };
        public event Action<string, string> FileNameProvided = (x, y) => { };


        public MainWindow()
        {
            InitializeComponent();

            StringDownloaded += (x) => SetControlStateAfterDownload();
            StringDownloaded += (x) => DownloadedString = x;
            StringDownloaded += (x) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DisplayText.Text = "Podaj nazwę pliku";

                    MessageBox.Show("Podaj nazwę pliku ty");
                });
            };

            FileNameProvided += SaveToFile;
            FileNameProvided += (x, y) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DisplayText.Text = "Zapisano plik!";

                    MessageBox.Show("Plik zapisano");
                });
            };

            FileName.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// The function that is fired when user clicks button
        /// </summary>
        /// <param name="sender">sender of function = button</param>
        /// <param name="e">EventArgs</param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadedString != null)
            {
                FileDownloadData.FileName = FileName.Text;

                if (ValidateData())
                {
                    FileNameProvided.Invoke(FileDownloadData.FileName, DownloadedString);

                }

                return;
            }

            FileDownloadData.Url = WebsiteUrl.Text;
            if (ValidateData())
            {
                await Task.Run(async () =>
                {
                    var webClient = new WebClient();

                    var downloadedString = await webClient.DownloadStringTaskAsync(FileDownloadData.Url);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DisplayText.Text = "Sukces";
                    });
                    Thread.Sleep(500);

                    StringDownloaded.Invoke(downloadedString);
                });

            }
           
        }

        private void SetControlStateAfterDownload()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                FileName.Visibility = Visibility.Visible;
                WebsiteUrl.Visibility = Visibility.Hidden;
                SubmitButton.Content = "Click to save";
            });
        }
        private void SaveToFile(string fileName, string downloadedString)
        {
            File.WriteAllText(fileName, downloadedString);
        }

        private bool ValidateData()
        {
            var validationContext = new ValidationContext(FileDownloadData);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(FileDownloadData, validationContext, results, true)) return true;
            foreach (var result in results)
            {
                MessageBox.Show(result.ErrorMessage);
            }
            return false;
        }
    }
}