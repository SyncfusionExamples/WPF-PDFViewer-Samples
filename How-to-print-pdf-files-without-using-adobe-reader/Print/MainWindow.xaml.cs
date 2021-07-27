using Syncfusion.Windows.PdfViewer;
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

namespace Print
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
            //Initialize PDF Viewer.

            PdfViewerControl pdfViewer1 = new PdfViewerControl();

            //Load the PDF.

            pdfViewer1.Load(@"../../Data/Windows Store Apps Succinctly.pdf");

            // Printing document using Print method

            pdfViewer1.Print();
        }
    }
}
