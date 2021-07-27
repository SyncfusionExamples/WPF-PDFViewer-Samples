using Syncfusion.Pdf.Parsing;
using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
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

namespace SignatureField
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Load the PDF document.
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument("../../Data/Sample.pdf");
            //Flatten the form.
            loadedDocument.Form.Flatten = true;

            MemoryStream memoryStream = new MemoryStream();
            //Save the modified document.
            loadedDocument.Save(memoryStream);
            memoryStream.Position = 0;

            PdfViewerControl pdfViewerControl = new PdfViewerControl();
            // load the document in pdfviewercontrol
            pdfViewerControl.Load(memoryStream);

            control.Children.Add(pdfViewerControl);

        }
    }
}
