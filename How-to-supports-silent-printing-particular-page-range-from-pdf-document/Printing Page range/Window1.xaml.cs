#region Copyright Syncfusion Inc. 2001 - 2014
// Copyright Syncfusion Inc. 2001 - 2014. All rights reserved.
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
using System.IO;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf;

namespace PrintPagerange
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        # region Private Members
        private PdfDocumentView pdfDocumentview;
        # endregion

        # region Constructor
        public Window1()
        {
            InitializeComponent();

            pdfDocumentview = new PdfDocumentView();
        }
        # endregion

        # region Events
        /// <summary>
        /// Handles selected / all pages option.
        /// </summary>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (allRadioBtn.IsChecked.Value)
                rangeStackPanel.IsEnabled = false;
            else
                rangeStackPanel.IsEnabled = true;
        }

        /// <summary>
        /// Prints all or selected pages.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int from = comboBoxFrom.SelectedIndex;
            int to = comboBoxTo.SelectedIndex;

            if (from > to)
            {
                MessageBox.Show("The 'From' value cannot be greater than the 'To' value.", "Error");
                return;
            }

            if (from == 0 && to == pdfDocumentview.PageCount - 1)
                //Prints all pages
                pdfDocumentview.Print();
            else
            {
                //Load the document in PdfLoaded document
                PdfLoadedDocument loadeddocument = new PdfLoadedDocument("../../Data/Manual.pdf");

                //create an instance of PdfDocument
                PdfDocument document = new PdfDocument();

                //Import the page you need to print to the pdfdocument
                document.ImportPageRange(loadeddocument, from, to);

                //save the document in a memory stream
                MemoryStream stream = new MemoryStream();
                document.Save(stream);

                //load the pdfdocumentview with the memory stream
                pdfDocumentview.Unload();
                pdfDocumentview.Load(stream);

                //Prints selected range of pages
                pdfDocumentview.Print();
            }
        }

        /// <summary>
        /// Loads PDF into PdfViewer.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pdfDocumentview.Load("../../Data/Manual.pdf");

            int count = pdfDocumentview.PageCount;
            for (int i = 1; i < count + 1; i++)
            {
                comboBoxFrom.Items.Add(i);
                comboBoxTo.Items.Add(i);
            }

            comboBoxFrom.SelectedIndex = 0;
            comboBoxTo.SelectedIndex = count - 1;
            rangeStackPanel.IsEnabled = false;
        }
        #endregion
    }
}