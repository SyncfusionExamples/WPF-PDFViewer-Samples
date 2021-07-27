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
using Microsoft.Win32;
using Syncfusion.Windows.PdfViewer;

namespace ToolbarCustomization_2008
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
            ImageSourceConverter converter = new ImageSourceConverter();

            // Position and size the window.
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth * 2 / 3;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * (3 / 2) - 50;
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth / 6;
            this.Top = 5;
        }
        # endregion

        # region Events
        /// <summary>
        /// Loads PDF on load and initializes controls in custom toolbar.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            First.Content = "<<";
            Previous.Content = "<";
            Next.Content = ">";
            Last.Content = ">>";
            pdfViewerControl.Load("../../Data/Barcode.pdf");
        }

        /// <summary>
        /// Handles file open in custom toolbar.
        /// </summary>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Pdf Files (.pdf)|*.pdf";
            dialog.ShowDialog();
            if (!string.IsNullOrEmpty(dialog.FileName))
                LoadDocument(dialog.FileName);
            pdfViewerControl.GoToPageAtIndex(1);
        }

        /// <summary>
        /// Handles first page navigation.
        /// </summary>
        private void First_Click(object sender, RoutedEventArgs e)
        {
            pdfViewerControl.GoToPageAtIndex(1);
        }

        /// <summary>
        /// Handles previous page navigation.
        /// </summary>
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (pdfViewerControl.CurrentPageIndex != 1)
            {
                pdfViewerControl.GoToPageAtIndex(pdfViewerControl.CurrentPageIndex - 1);
            }
        }

        /// <summary>
        /// Handles next page navigation.
        /// </summary>
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (pdfViewerControl.CurrentPageIndex != pdfViewerControl.PageCount)
            {
                pdfViewerControl.GoToPageAtIndex(pdfViewerControl.CurrentPageIndex + 1);
            }
        }

        /// <summary>
        /// Handles last page navigation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Last_Click(object sender, RoutedEventArgs e)
        {
            pdfViewerControl.GoToPageAtIndex(pdfViewerControl.PageCount);
        }

        /// <summary>
        /// Handles FitPage Functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fitPage_Click(object sender, RoutedEventArgs e)
        {
            
            if (pdfViewerControl.ZoomMode != ZoomMode.FitPage)
                pdfViewerControl.ZoomMode = ZoomMode.FitPage;
        }

        /// <summary>
        ///  Handles FitWidth Functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void fitWidth_Click(object sender, RoutedEventArgs e)
        {
            if (pdfViewerControl.ZoomMode != ZoomMode.FitWidth)
                pdfViewerControl.ZoomMode = ZoomMode.FitWidth;
        }
        # endregion

        #region Helper methods
        private void LoadDocument(string fileName)
        {
            pdfViewerControl.Load(fileName);
        }
        #endregion

    }
}
