using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Windows.Shared;

namespace VectorPrinting_2008
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ChromelessWindow
    {
        # region Private Members
        private PdfDocumentView pdfViewer1;
        # endregion

        # region Constructor
        public Window1()
        {
            InitializeComponent();
            ImageSourceConverter converter = new ImageSourceConverter();
            this.Icon = (ImageSource)converter.ConvertFromString("../../Data/pdf viewer.png");
            this.image1.Source = (ImageSource)converter.ConvertFromString("../../Data/pdf_header.png");

            pdfViewer1 = new PdfDocumentView();
            SkinStorage.SetMetroBrush(this, new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)));
        }
        # endregion

        # region Events
        /// <summary>
        /// Prints the select pages.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pdfViewer1.Print();
        }

        /// <summary>
        /// Loads PDF into PdfViewer.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pdfViewer1.Load("../../Data/Barcode.pdf");
        }
        #endregion
    }
}