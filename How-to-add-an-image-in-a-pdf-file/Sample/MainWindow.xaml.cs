using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PdfLoadedDocument loadedDocument;
        PdfUnitConvertor pdfUnitConvertor;
        bool m_addImage = false;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadedDocument = new PdfLoadedDocument("../../Data/GIS Succinctly.pdf");
            pdfUnitConvertor = new PdfUnitConvertor();
            //Load the document
            pdfViewer.Load(loadedDocument);
            pdfViewer.PageClicked += PdfViewer_PageClicked;
        }
        private void PdfViewer_PageClicked(object sender, Syncfusion.Windows.PdfViewer.PageClickedEventArgs args)
        {
            //Gets the point where an image to be add
            System.Windows.Point point = args.Position;
            PdfImage newImage = PdfImage.FromFile("../../Data/NWTraders.jpeg");
            //point conversion
            float x = pdfUnitConvertor.ConvertFromPixels((float)args.Position.X, PdfGraphicsUnit.Point);
            float y = pdfUnitConvertor.ConvertFromPixels((float)args.Position.Y, PdfGraphicsUnit.Point);
            PointF point1 = new PointF(x, y);

            if (m_addImage)
            {
                //Draw an image 
                loadedDocument.Pages[args.PageIndex].Graphics.DrawImage(newImage, point1);
                MemoryStream memoryStream = new MemoryStream();
                //Save the modified document.
                loadedDocument.Save(memoryStream);
                memoryStream.Position = 0;
                //Load the modified document.
                pdfViewer.Load(memoryStream);
                pdfViewer.GoToPageAtIndex(args.PageIndex + 1);
            }
            m_addImage = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m_addImage = true;
        }
       
    }
}
