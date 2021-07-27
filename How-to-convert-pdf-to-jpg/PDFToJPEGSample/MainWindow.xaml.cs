using Syncfusion.Windows.PdfViewer;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System;
using System.Windows.Media.Imaging;
using System.IO;
using Syncfusion.Pdf.Parsing;

namespace PDFToJPEGSample
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
            //Export the images From the input PDF file at the page range of 0 to 1 .
            BitmapSource[] image = pdfViewer.ExportAsImage(0, loadedDocument.Pages.Count - 1);
            //Setup the output path
            string output = @"..\..\Output\Image";
            if (image != null)
            {
                for (int i = 0; i < image.Length; i++)
                {
                    //Initialize the new Jpeg bitmap encoder
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    //Create the bitmap frame using the bitmap source and add it to the encoder.
                    encoder.Frames.Add(BitmapFrame.Create(image[i]));
                    //Create the file stream for the output in the desired image format.
                    FileStream stream = new FileStream(output + i.ToString() + ".Jpeg", FileMode.Create);
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
