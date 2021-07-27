using System.Windows.Media.Imaging;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Pdf.Parsing;
using System.Windows;
using System.IO;

namespace PDFToPNGSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PdfViewerControl pdfViewer = new PdfViewerControl();
            //Load the input PDF file
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument("../../Data/Barcode.pdf");
            pdfViewer.Load(loadedDocument);
            //Export all the pages as images at the specific page range.
            BitmapSource[] image = pdfViewer.ExportAsImage(0, loadedDocument.Pages.Count - 1);
            //Setup the output path
            string output = @"..\..\Output\Image";
            if (image != null)
            {
                for (int i = 0; i < image.Length; i++)
                {
                    //Initialize the new PngBitmapEncoder
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    //Create the bitmap frame using the bitmap source and add it to the encoder.
                    encoder.Frames.Add(BitmapFrame.Create(image[i]));
                    //Create the file stream for the output in the desired image format.
                    FileStream stream = new FileStream(output + i.ToString() + ".png", FileMode.Create);
                    //Save the stream, so that the image will be generated in the output location.
                    encoder.Save(stream);
                }
            }
            //Dispose the document.
            loadedDocument.Dispose();
            loadedDocument = null;
        }
    }
}
