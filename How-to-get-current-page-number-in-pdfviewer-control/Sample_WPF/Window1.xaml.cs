#region Copyright Syncfusion Inc. 2001 - 2014
// Copyright Syncfusion Inc. 2001 - 2014. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GettingStarted_2008
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        # region Private Members
        DispatcherTimer timer = new DispatcherTimer();
        # endregion

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
            pdfviewer1.CurrentPageChanged += pdfviewer1_CurrentPageChanged;
            pdfviewer1.Load("../../Data/GIS Succinctly.pdf");  
        }

        void pdfviewer1_CurrentPageChanged(object sender, EventArgs args)
        {
            pagenumberTxtbox.Text = (sender as PdfDocumentView).CurrentPageIndex.ToString();
        }
        # endregion
    }
}
