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
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Syncfusion.PdfViewer.WPF;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Pdf.Parsing;
using System.IO;
using System.Net;

namespace GettingStarted_2008
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        GettingStarted_2008.UI.CustomToolBar ctool;
        # region Constructor
        public Window1()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
           
        }

        # endregion

        # region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            byte[] myDataBuffer = client.DownloadData((new Uri("http://www.telmi.lt/wp-content/uploads/2013/02/Simple.pdf")));

            MemoryStream storeStream = new MemoryStream();

            storeStream.SetLength(myDataBuffer.Length);
            storeStream.Write(myDataBuffer, 0, (int)storeStream.Length);
            pdfViewer.Load(storeStream);
            storeStream.Flush();
        }

        # endregion
        
    }
}
