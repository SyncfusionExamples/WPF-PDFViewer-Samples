#region Copyright Syncfusion Inc. 2001-2016.
// Copyright Syncfusion Inc. 2001-2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
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
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;
using System.Drawing;
using System.IO;

namespace GettingStarted_2008
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ChromelessWindow
    {
        # region Private Members
        private string fileName;
        private string filePath;
        # endregion

        # region Constructor
        public Window1()
        {
            InitializeComponent();
            ImageSourceConverter converter = new ImageSourceConverter();
            // Position and size the window.
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth * 2 / 3;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * (3 / 2) - 50;
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth / 6;
            this.Top = 5;
            this.WindowState = WindowState.Maximized;
            this.UseNativeChrome = true;
        }
        # endregion

        # region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Create a new PDF document.
            PdfDocument document = new PdfDocument();

            //Add a page to the document.
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page.
            PdfGraphics graphics = page.Graphics;

            //Set the standard font.
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Draw the text.
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));

            //Save the document to Stream.
            Stream fileStream = new MemoryStream();
            document.Save(fileStream);

            //Close the document.
            document.Close(true);

            fileStream.Position = 0;

            // Load the PDF file stream
            pdfViewer.Load(fileStream);
        }
        # endregion
    }
}
