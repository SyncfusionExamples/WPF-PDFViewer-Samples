using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

namespace SampleWPF
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
        int thumbnailZoomFactor = 4;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PdfViewer.Load(@"..\..\Data\HTTP Succinctly.pdf");
            ThumbnailGrid.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237))))));
            PdfViewer.DocumentLoaded += PdfViewer_DocumentLoaded;
            
        }

        private void PdfViewer_DocumentLoaded(object sender, EventArgs args)
        {
            ThumbnailGrid.RowDefinitions.Clear();
            
            ExportAsImage();
        }

        private async void ExportAsImage()
        {
            int count = PdfViewer.LoadedDocument.Pages.Count;
            float height = PdfViewer.LoadedDocument.Pages[0].Size.Height / thumbnailZoomFactor;
            float width = PdfViewer.LoadedDocument.Pages[0].Size.Width / thumbnailZoomFactor;
            ParentGrid.ColumnDefinitions[0].Width = new GridLength(width + 30);
            for (int i=0;i< PdfViewer.LoadedDocument.Pages.Count;i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(height+5);
                ThumbnailGrid.RowDefinitions.Add(row);
                Bitmap bitMap = new Bitmap(await Task.Run(() => PdfViewer.LoadedDocument.ExportAsImage(i)), (int)width,(int)height);
                var memory = new MemoryStream();
                bitMap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage=new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                System.Windows.Controls.Image image = new System.Windows.Controls.Image()
                {
                    Source = bitmapImage
                };
                image.Height = height;
                image.Width = width;
                image.MouseUp += Image_MouseUp;
                Grid.SetRow(image, i);
                ThumbnailGrid.Children.Add(image);
               
            }
            
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            int pageNumber = (int)img.GetValue(Grid.RowProperty);
            PdfViewer.GoToPageAtIndex(pageNumber+1);
        }
    }
}
