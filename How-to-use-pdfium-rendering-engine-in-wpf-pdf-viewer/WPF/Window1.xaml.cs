#region Copyright Syncfusion Inc. 2001 - 2018
// Copyright Syncfusion Inc. 2001 - 2018. All rights reserved.
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
using System.IO;

namespace GettingStarted_2008
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : ChromelessWindow
    {
	    //Local path to locate Pdfium folder
        string pdfiumPath = @"D:\";
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Set the reference path of Pdfium assemblies
            pdfViewerControl1.ReferencePath = pdfiumPath;
            //Set the rendering engine of PDF viewer to Pdfium
            pdfViewerControl1.RenderingEngine = Syncfusion.Windows.PdfViewer.PdfRenderingEngine.Pdfium;
            if (Directory.Exists(pdfiumPath + "Pdfium"))
            {
                //Load the PDF document in PDF viewer.
                pdfViewerControl1.Load(@"../../Data/PDF_Succinctly.pdf");
            }
            else
            {
                //Display the error message if the Pdfium reference path is invalid. 
                MessageBox.Show("Please provide a valid Pdfium reference path", "Invalid reference path");
                this.Close();
            }
        }
    }
}
