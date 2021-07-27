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
using Syncfusion.PdfViewer.Base;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Windows.PdfViewer;

namespace GettingStarted_2008.UI
{
    
        
    /// <summary>
    /// Interaction logic for CustomToolBar.xaml
    /// </summary>
    public partial class CustomToolBar : UserControl
    {
        PdfDocumentView m_activeView;
        int[] zoomValues = new int[]{
           10, 25, 50, 75, 100, 125, 150, 200, 400, 800, 1600, 2400, 3200, 6400};
        string filename = string.Empty;
        public CustomToolBar()
        {
            InitializeComponent();
            AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
        }
        internal void Reset()
        {
            lblTotalPageCount.Text = "0";
            txtCurrentPageIndex.Text = "1";
            cmbCurrentZoomLevel.IsEnabled = false;
            btnFitPage.IsEnabled = false;
            btnFitWidth.IsEnabled = false;
            btnZoomIn.IsEnabled = false;
            btnZoomOut.IsEnabled = false;
            m_activeView.LoadedDocument = null;
        }

        #region Properties
        public PdfDocumentView ActiveView
        {
            get
            {
                return m_activeView;
            }
            set
            {
                if (m_activeView != value)
                {
                    m_activeView = value;
                    Initialize(value);
                }
            }
        }
        #endregion

        #region Implementation
        public void Initialize(PdfDocumentView view)
        {
            UnWireEvents();
            WireUpEvents();
            m_activeView = view;
            if (m_activeView.LoadedDocument == null)
                this.cmbCurrentZoomLevel.IsEnabled = false;
            else
                this.cmbCurrentZoomLevel.IsEnabled = true;
            m_activeView.NavigationButtonStatesChanged += m_activeView_NavigationButtonStatesChanged;
            m_activeView.CurrentPageChanged += m_activeView_CurrentPageChanged;

            txtCurrentPageIndex.Text = (m_activeView.CurrentPageIndex + 1).ToString();
            lblTotalPageCount.Text = m_activeView.PageCount.ToString();


            m_activeView_NavigationButtonStatesChanged(null, null);

            m_activeView.ZoomChanged += new PdfDocumentView.ZoomChangedEventHandler(m_activeView_ZoomChanged);
            cmbCurrentZoomLevel.SelectedItem = cmbCurrentZoomLevel.Items[4];
        }

        void m_activeView_ZoomChanged(object sender, ZoomEventArgs args)
        {
            cmbCurrentZoomLevel.Text = string.Format("{0}%", args.ZoomPercentage);
        }

        void m_activeView_CurrentPageChanged(object sender, EventArgs args)
        {
            txtCurrentPageIndex.Text = m_activeView.CurrentPageIndex.ToString();
            //SetCurrentPageIndex(m_activeView.CurrentPageIndex + 1);
        }

        void m_activeView_NavigationButtonStatesChanged(object sender, EventArgs args)
        {
            btnGoToFirstPage.IsEnabled = m_activeView.CanGoToFirstPage;
            btnGoToLastPage.IsEnabled = m_activeView.CanGoToLastPage;
            btnGoToNextPage.IsEnabled = m_activeView.CanGoToNextPage;
            btnGoToPreviousPage.IsEnabled = m_activeView.CanGoToPreviousPage;
        }

        void SetCurrentPageIndex(int index)
        {
            if (m_activeView != null)
            {
                if (index <= m_activeView.PageCount)
                    txtCurrentPageIndex.Text = index.ToString();
            }
        }
        #endregion


        #region Helper Methods
        void WireUpEvents()
        {
            btnGoToFirstPage.Click += new RoutedEventHandler(btnGoToFirstPage_Click);
            btnGoToPreviousPage.Click += new RoutedEventHandler(btnGoToPreviousPage_Click);
            btnGoToNextPage.Click += new RoutedEventHandler(btnGoToNextPage_Click);
            btnGoToLastPage.Click += new RoutedEventHandler(btnGoToLastPage_Click);
            btnPrint.Click += new RoutedEventHandler(btnPrint_Click);
            txtCurrentPageIndex.KeyDown += new KeyEventHandler(txtCurrentPageIndex_KeyDown);

            btnGoToFirstPage.IsEnabledChanged += new DependencyPropertyChangedEventHandler(btnGoToFirstPage_EnabledChanged);
            btnGoToLastPage.IsEnabledChanged += new DependencyPropertyChangedEventHandler(btnGoToLastPage_EnabledChanged);
            btnGoToNextPage.IsEnabledChanged += new DependencyPropertyChangedEventHandler(btnGoToNextPage_EnabledChanged);
            btnGoToPreviousPage.IsEnabledChanged += new DependencyPropertyChangedEventHandler(btnGoToPreviousPage_EnabledChanged);

            btnFitPage.Click += new RoutedEventHandler(btnFitPage_Click);
            btnFitWidth.Click += new RoutedEventHandler(btnFitWidth_Click);
            btnZoomIn.Click += new RoutedEventHandler(btnZoomIn_Click);
            btnZoomOut.Click += new RoutedEventHandler(btnZoomOut_Click);

            cmbCurrentZoomLevel.SelectionChanged += new SelectionChangedEventHandler(cmbCurrentZoomLevel_SelectionChanged);
            cmbCurrentZoomLevel.KeyDown += new KeyEventHandler(cmbCurrentZoomLevel_KeyDown);
        }

        void cmbCurrentZoomLevel_KeyDown(object sender, KeyEventArgs e)
        {
            ComboBox zoomBox = sender as ComboBox;
            if (e.Key == Key.Enter)
            {
                string zoomEntered = zoomBox.Text;
                int magnificationValue;
                if (zoomEntered.Contains("%"))
                {
                    int index = zoomEntered.IndexOf('%');
                    zoomEntered = zoomEntered.Substring(0, index);
                }
                int.TryParse(zoomEntered, out magnificationValue);
                if (magnificationValue < 10)
                    magnificationValue = 10;
                if (magnificationValue > 6400)
                    magnificationValue = 6400;

                m_activeView.ZoomTo(magnificationValue);
                cmbCurrentZoomLevel.Text = magnificationValue.ToString() + "%";
            }
        }

        void UnWireEvents()
        {
            btnGoToFirstPage.Click -= new RoutedEventHandler(btnGoToFirstPage_Click);
            btnGoToPreviousPage.Click -= new RoutedEventHandler(btnGoToPreviousPage_Click);
            btnGoToNextPage.Click -= new RoutedEventHandler(btnGoToNextPage_Click);
            btnGoToLastPage.Click -= new RoutedEventHandler(btnGoToLastPage_Click);
            btnPrint.Click -= new RoutedEventHandler(btnPrint_Click);
            txtCurrentPageIndex.KeyDown -= new KeyEventHandler(txtCurrentPageIndex_KeyDown);

            btnGoToFirstPage.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(btnGoToFirstPage_EnabledChanged);
            btnGoToLastPage.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(btnGoToLastPage_EnabledChanged);
            btnGoToNextPage.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(btnGoToNextPage_EnabledChanged);
            btnGoToPreviousPage.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(btnGoToPreviousPage_EnabledChanged);

            btnFitPage.Click -= new RoutedEventHandler(btnFitPage_Click);
            btnFitWidth.Click -= new RoutedEventHandler(btnFitWidth_Click);
            btnZoomIn.Click -= new RoutedEventHandler(btnZoomIn_Click);
            btnZoomOut.Click -= new RoutedEventHandler(btnZoomOut_Click);

            cmbCurrentZoomLevel.SelectionChanged -= new SelectionChangedEventHandler(cmbCurrentZoomLevel_SelectionChanged);
            cmbCurrentZoomLevel.KeyDown -= new KeyEventHandler(cmbCurrentZoomLevel_KeyDown);
        }
        
        void cmbCurrentZoomLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCurrentZoomLevel.SelectedItem != null)
            {
                string text = cmbCurrentZoomLevel.SelectedItem.ToString();
                text = text.TrimEnd(new char[] { '%' });
                int zoomLevel = 100;

                if (int.TryParse(text, out zoomLevel))
                {
                    m_activeView.ZoomTo(zoomLevel);
                }
            }
        }

        void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (this.m_activeView.LoadedDocument == null)
            {
                return;
            }
            int currentZoomLevel = GetCurrentZoomLevel();
            int index = GetComboBoxItemIndex(string.Format("{0}%", currentZoomLevel));
            if (index - 1 >= 0)
            {
                cmbCurrentZoomLevel.SelectedIndex = index - 1;
            }
            else
            {
                for (int i = 0; i < zoomValues.Length; i++)
                {
                    if (i + 1 < zoomValues.Length)
                    {
                        if (currentZoomLevel >= zoomValues[i] && currentZoomLevel < zoomValues[i + 1])
                        {
                            cmbCurrentZoomLevel.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }

            string zoomEntered = cmbCurrentZoomLevel.Text;
            int magnificationValue;
            if (zoomEntered.Contains("%"))
            {
                int index1 = zoomEntered.IndexOf('%');
                zoomEntered = zoomEntered.Substring(0, index1);
            }
            int.TryParse(zoomEntered, out magnificationValue);
            if (magnificationValue < 10)
                magnificationValue = 10;
            if (magnificationValue > 6400)
                magnificationValue = 6400;

            m_activeView.ZoomTo(magnificationValue);
        }

        void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (this.m_activeView.LoadedDocument == null)
            {
                return;
            }
            int currentZoomLevel = GetCurrentZoomLevel();
            int index = GetComboBoxItemIndex(string.Format("{0}%", currentZoomLevel));
            if (index == 0 || (index - 1 >= 0 && index + 1 < cmbCurrentZoomLevel.Items.Count))
            {
                cmbCurrentZoomLevel.SelectedIndex = index + 1;
            }
            else
            {
                for (int i = 0; i < zoomValues.Length; i++)
                {
                    if (i - 1 >= 0)
                    {
                        if (currentZoomLevel <= zoomValues[i] && currentZoomLevel > zoomValues[i - 1])
                        {
                            cmbCurrentZoomLevel.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }

            string zoomEntered = cmbCurrentZoomLevel.Text;
            int magnificationValue;
            if (zoomEntered.Contains("%"))
            {
                int index1 = zoomEntered.IndexOf('%');
                zoomEntered = zoomEntered.Substring(0, index1);
            }
            int.TryParse(zoomEntered, out magnificationValue);
            if (magnificationValue < 10)
                magnificationValue = 10;
            if (magnificationValue > 6400)
                magnificationValue = 6400;

            m_activeView.ZoomTo(magnificationValue);
        }

        int GetComboBoxItemIndex(string text)
        {
            for (int i = 0; i < cmbCurrentZoomLevel.Items.Count; i++)
            {
                if (string.Equals(
                    (cmbCurrentZoomLevel.Items[i] as ComboBoxItem).Content, text))
                    return i;
            }

            return -1;
        }

        int GetCurrentZoomLevel()
        {
            string text = "";
            if (cmbCurrentZoomLevel.SelectedValue == null)
                text = cmbCurrentZoomLevel.Text;
            else
                text = (cmbCurrentZoomLevel.SelectedValue as ComboBoxItem)
               .Content.ToString();

            text = text.TrimEnd(new char[] { '%' });
            int zoomLevel = 100;

            if (int.TryParse(text, out zoomLevel))
            {
                return zoomLevel;
            }

            return 100;
        }

        void btnFitWidth_Click(object sender, RoutedEventArgs e)
        {
            m_activeView.ZoomMode = ZoomMode.FitWidth;
        }

        void btnFitPage_Click(object sender, RoutedEventArgs e)
        {
            m_activeView.ZoomMode = ZoomMode.FitPage;
        }

        void btnGoToPreviousPage_EnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Image buttonImage = btnGoToPreviousPage.Content as Image;
            buttonImage.Source =
                (btnGoToPreviousPage.IsEnabled)
                    ? this.Resources["GoToPreviousPage_Enabled"] as ImageSource
                    : this.Resources["GoToPreviousPage_Disabled"] as ImageSource;
        }

        void btnGoToNextPage_EnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Image buttonImage = btnGoToNextPage.Content as Image;
            buttonImage.Source =
                (btnGoToNextPage.IsEnabled)
                    ? this.Resources["GoToNextPage_Enabled"] as ImageSource
                    : this.Resources["GoToNextPage_Disabled"] as ImageSource;
        }

        void btnGoToLastPage_EnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Image buttonImage = btnGoToLastPage.Content as Image;
            buttonImage.Source =
                (btnGoToLastPage.IsEnabled)
                    ? this.Resources["GoToLastPage_Enabled"] as ImageSource
                    : this.Resources["GoToLastPage_Disabled"] as ImageSource;
        }

        void btnGoToFirstPage_EnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Image buttonImage = btnGoToFirstPage.Content as Image;
            buttonImage.Source =
                (btnGoToFirstPage.IsEnabled)
                    ? this.Resources["GoToFirstPage_Enabled"] as ImageSource
                    : this.Resources["GoToFirstPage_Disabled"] as ImageSource;
        }

        int GetCurrentPageIndex()
        {
            if (string.IsNullOrEmpty(txtCurrentPageIndex.Text))
                return 1;

            else
            {
                int index = 1;
                if (int.TryParse(txtCurrentPageIndex.Text, out index))
                    return index - 1;
            }

            return -1;
        }


        void txtCurrentPageIndex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && m_activeView.LoadedDocument != null)
            {
                m_activeView.GoToPageAtIndex(
                    GetCurrentPageIndex());
            }
        }


        void btnPrint_Click(object sender, EventArgs e)
        {
           m_activeView.Print();
        }

        void btnGoToLastPage_Click(object sender, EventArgs e)
        {
            m_activeView.GoToLastPage();
        }

        void btnGoToNextPage_Click(object sender, EventArgs e)
        {
            m_activeView.GoToNextPage();
        }

        void btnGoToPreviousPage_Click(object sender, EventArgs e)
        {
            m_activeView.GoToPreviousPage();
        }

        void btnGoToFirstPage_Click(object sender, EventArgs e)
        {
            m_activeView.GoToFirstPage();
        }

        //void Print()
        //{
        //    if (m_activeView.LoadedDocument != null)
        //    {
        //        System.Windows.Forms.PrintDialog dialog = new System.Windows.Forms.PrintDialog();

        //        int pageCount = m_activeView.PageCount;
        //        dialog.AllowPrintToFile = true;
        //        dialog.AllowSomePages = true;
        //        dialog.PrinterSettings.FromPage = 1;
        //        dialog.PrinterSettings.ToPage = pageCount;
        //        dialog.PrinterSettings.MaximumPage = pageCount;
        //        dialog.PrinterSettings.MinimumPage = 1;
        //        dialog.UseEXDialog = true;
        //        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            m_activeView.PrintFromPage = dialog.PrinterSettings.FromPage;
        //            m_activeView.PrintToPage = dialog.PrinterSettings.ToPage;
        //            System.Drawing.Printing.PrinterSettings.PaperSizeCollection collection = dialog.PrinterSettings.PaperSizes;
        //            string sourceName = dialog.PrinterSettings.PaperSources[0].SourceName;
        //            int rawKind = dialog.PrinterSettings.PaperSources[0].RawKind;

        //            foreach (System.Drawing.Printing.PaperSize size in collection)
        //            {
        //                if (size.RawKind == rawKind)
        //                {

        //                    m_activeView.PrintHeight = size.Height;
        //                    m_activeView.PrintWidth = size.Width;
        //                }
        //            }

        //            System.Drawing.Printing.PrintDocument document = m_activeView.PrintingDocument; //m_activeView.PrintDocument;
        //            document.PrinterSettings = dialog.PrinterSettings;
        //            for (int i = 0; i < dialog.PrinterSettings.Copies; i++)
        //            {
        //                m_activeView.CurrentPageOnPrint = dialog.PrinterSettings.FromPage - 1;
        //                document.Print();
        //            }
        //        }
        //    }
        //}
        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }
        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);
            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    textBox.Focus();
                    e.Handled = true;
                }
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                e.Handled = true;
            }
        }
        #endregion
       
    }
}
