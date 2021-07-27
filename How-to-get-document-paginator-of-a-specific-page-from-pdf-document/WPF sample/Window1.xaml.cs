#region Copyright Syncfusion Inc. 2001 - 2012
// Copyright Syncfusion Inc. 2001 - 2012. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System.Windows;
using System.Windows.Documents;
using Syncfusion.Pdf.Parsing;
using System.IO;
using Syncfusion.Pdf;
using System.Windows.Xps.Packaging;
using Syncfusion.Windows.PdfViewer;

namespace GettingStarted_2008
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {       
        # region Constructor
        public Window1()
        {
            InitializeComponent();           
        }
        # endregion

        # region Events

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            PdfDocumentView pdfViewer = new PdfDocumentView();

            //Load the document in PdfLoadedDocument
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument("../../Data/Objective C succinity.pdf");

            //create an instance of PdfDocument
            PdfDocument document = new PdfDocument();

            //Import the page you need to get as document paginator to the pdfdocument
            document.ImportPage(loadedDocument, 4);

            //save the document in a memory stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            //load the pdfdocumentview with the memory stream
            pdfViewer.Load(stream);
            try
            {
                //get fixed document from viewer
                FixedDocument pdfPrintDocument = pdfViewer.PrintDocument;
                
                //get document paginator from fixed document
                DocumentPaginator pdfPaginator = pdfPrintDocument.DocumentPaginator;
               
                //write the document pageinator as xps document.
                var xpsDocument = new XpsDocument("../../Data/output.xps", FileAccess.Write);
                var documentWriter = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                documentWriter.Write(pdfPaginator);
                xpsDocument.Close();
            }

            finally
            {
                pdfViewer.Unload();
            }
        }
            
        # endregion

       
    }
}
