using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

namespace Random_images
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
        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string category = categoryTextBox.Text;
            int width, height;

            if (!int.TryParse(widthTextBox.Text, out width) || !int.TryParse(heightTextBox.Text, out height))
            {
                MessageBox.Show("Enter height and width.");
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "image|*.jpg",
                Title = "SaveImage"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string imageUrl = $"https://source.unsplash.com/random/{width}x{height}/?{category}&1";

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                        using (var imageStream = new MemoryStream(imageBytes))
                        using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            await imageStream.CopyToAsync(fileStream);
                            MessageBox.Show("Image was saved.");
                        }
                    }
                    catch (HttpRequestException)
                    {
                        MessageBox.Show("Error.");
                    }
                }
            }
        }
    }
}
