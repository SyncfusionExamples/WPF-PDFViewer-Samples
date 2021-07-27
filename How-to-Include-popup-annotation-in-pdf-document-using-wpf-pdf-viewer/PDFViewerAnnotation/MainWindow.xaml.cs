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
using Microsoft.Win32;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Windows.Diagram;
using Syncfusion.Windows.PdfViewer;
using System.Drawing.Drawing2D;
using Syncfusion.Pdf.Interactive;

namespace PDFViewerAnnotation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region fileds
        PdfDocumentView viewer;
        BitmapSource source;
        PdfLoadedDocument ldoc;
        PdfUnitConvertor converter = new PdfUnitConvertor();
        int currentPageIndex = 0;
        float pageWidth;
        float pageHeight;
        private PdfUnitConvertor m_unitConvertor = new PdfUnitConvertor();
        Node m_SelectedNode = null;
     
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            //Remove unnecessary palettes like shapes, images from the SymbolPalette of the DiagramControl
            diagramControl1.SymbolPalette.SymbolGroups.Remove(diagramControl1.SymbolPalette.SymbolGroups[4]);
            diagramControl1.SymbolPalette.SymbolGroups.Remove(diagramControl1.SymbolPalette.SymbolGroups[3]);
            diagramControl1.SymbolPalette.SymbolGroups.Remove(diagramControl1.SymbolPalette.SymbolGroups[2]);
            diagramControl1.SymbolPalette.SymbolGroups.Remove(diagramControl1.SymbolPalette.SymbolGroups[1]);
            diagramControl1.SymbolPalette.SymbolGroups.Remove(diagramControl1.SymbolPalette.SymbolGroups[0]);
            diagramControl1.SymbolPalette.BorderBrush = Brushes.MidnightBlue;
            diagramControl1.SymbolPalette.SymbolPaletteGroupForeground = Brushes.White;
            //Loading palette items
            LoadSymbolPalette();
            //Loading the document
            LoadDocument("../../Data/Product Line Sales.pdf");
        }
        #region Events
        //Node drop event
        private void Node_Dropevent(object sender, NodeDroppedRoutedEventArgs evtArgs)
        {
            //getting the node name
            string s = diagramControl1.SymbolPalette.SelectedItem.ItemName;
            string[] words = s.Split('_');
            Node node = (evtArgs.DroppedNode as Node);
            if (words[0].Equals("PART"))
            {
                node.Tag = words[0];
            }
            else
                node.Tag = words[1];
           (node.Content as TextBox).IsHitTestVisible = true;
            

        }
        //used to open the document
        private void New_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".pdf";
            dialog.Filter = "Adobe Pdf Documents(.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                LoadDocument(dialog.FileName);

            }
        }
        //used to save the document
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DrawShapes();

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "sample.pdf"; // Default file name
            dlg.DefaultExt = ".Pdf"; // Default file extension
            dlg.Filter = "Adobe PDF(.Pdf)|*.PDF"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                ldoc.Save(filename);
                ldoc.Close();
            }
        }
        private void Selected_Node(object sender, NodeRoutedEventArgs evtArgs)
        {
            m_SelectedNode = evtArgs.Node;
        }
        private void diagramView1_NodeResized(object sender, NodeRoutedEventArgs evtArgs)
        {
            Node n = evtArgs.Node;
            TextBox text = n.Content as TextBox;
            if (text != null)
            {
                text.Width = n.Width - 10;
                text.Height = n.Height - 10;
            }
        }
        #endregion
        #region Helper Methods
        //Loading the document
        private void LoadDocument(string fileName)
        {
            try
            {
                //Load the pdf document
                ldoc = new PdfLoadedDocument(fileName);

                //Convert page units to pixels and create drawing surface.
                pageWidth = converter.ConvertToPixels(ldoc.Pages[0].Size.Width, PdfGraphicsUnit.Point);
                pageHeight = converter.ConvertToPixels(ldoc.Pages[0].Size.Height, PdfGraphicsUnit.Point);


                //Export the first page to the canvas.
                viewer = new PdfDocumentView();
                viewer.Load(ldoc);
                source = viewer.ExportAsImage(currentPageIndex);
                ImageBrush brush = new ImageBrush(source);
                diagramView1.SizeToContent = false;
                diagramView1.PageBackground = brush;
                diagramView1.BoundaryConstraintsArea = new Rect(0, 0, pageWidth, pageHeight);

            }
            catch
            {
                MessageBox.Show("An error occured while loading the document. Please contact Syncfusion support", "An Error Occured!!");
            }

        }

        //Inserting the text box in the PDF
        private void DrawShapes()
        {
            //Iterating the each node in the diagram view
            foreach (Node node in diagramModel1.Nodes)
            {
                Point position = new Point(node.OffsetX+2.3, node.OffsetY+2.3);
                double nodeWidth = node.Width;
                double nodeHeight = node.Height;
               
                System.Drawing.PointF StartInPoint = new System.Drawing.PointF(converter.ConvertFromPixels((float)position.X, PdfGraphicsUnit.Point), converter.ConvertFromPixels((float)position.Y, PdfGraphicsUnit.Point));
                System.Drawing.SizeF EndInPoint = new System.Drawing.SizeF(converter.ConvertFromPixels((float)nodeWidth, PdfGraphicsUnit.Point), converter.ConvertFromPixels((float)nodeHeight, PdfGraphicsUnit.Point));
                double angle = node.RotateAngle;
               
                //set the angle 
                if (angle != 0.0)
                {
                    StartInPoint.X = 0;
                    StartInPoint.Y = 0;
                }

                TextBox textBox = node.Content as TextBox;
                System.Drawing.RectangleF layoutRect = new System.Drawing.RectangleF(0,0,10,10);
                //Create a new popup annotation.
                PdfPopupAnnotation popupAnnotation = new PdfPopupAnnotation(layoutRect, textBox.Text);
                popupAnnotation.Location =new System.Drawing.PointF(StartInPoint.X, StartInPoint.Y);
                popupAnnotation.Border.Width = 4;
                popupAnnotation.Border.HorizontalRadius = 20;
                popupAnnotation.Border.VerticalRadius = 30;
                //Set the pdf popup icon.
                popupAnnotation.Icon = PdfPopupIcon.Note;
                //Add this annotation to a new page.
                ldoc.Pages[currentPageIndex].Annotations.Add(popupAnnotation);
                break;
                    
            }
        }
        //Loading the symbol palette
        private void LoadSymbolPalette()
        {
            SymbolPaletteGroup group = new SymbolPaletteGroup();

            group.Label = "Custom";

            SymbolPalette.SetFilterIndexes(group, new Int32Collection(new int[] { 0, 6 }));

            diagramControl1.SymbolPalette.SymbolGroups.Add(group);

            SymbolPaletteItem item = new SymbolPaletteItem();

            // Group added as SymbolPaletteItem's Content
            TextBox text = new TextBox() { Width = 400 };
            text.TextWrapping = TextWrapping.Wrap;

            text.Text = "Text Block";
            item.Content = text;
            (item.Content as TextBox).IsHitTestVisible = false;
            item.ItemName = "Text_TextBlock";
            item.Width = 150;

            group.Items.Add(item);
        }

        private void mnuNext_Click(object sender, RoutedEventArgs e)
        {            
            if (currentPageIndex < ldoc.Pages.Count-1)
            {
                currentPageIndex++;
                //Convert page units to pixels and create drawing surface.
                pageWidth = converter.ConvertToPixels(ldoc.Pages[0].Size.Width, PdfGraphicsUnit.Point);
                pageHeight = converter.ConvertToPixels(ldoc.Pages[0].Size.Height, PdfGraphicsUnit.Point);


                //Export the first page to the canvas.
                viewer = new PdfDocumentView();
                viewer.Load(ldoc);
                source = viewer.ExportAsImage(currentPageIndex);
                ImageBrush brush = new ImageBrush(source);
                diagramView1.SizeToContent = false;
                diagramView1.PageBackground = brush;
                diagramView1.BoundaryConstraintsArea = new Rect(0, 0, pageWidth, pageHeight);
            }
        }

        private void mnuPrev_Click(object sender, RoutedEventArgs e)
        {            
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                //Convert page units to pixels and create drawing surface.
                pageWidth = converter.ConvertToPixels(ldoc.Pages[0].Size.Width, PdfGraphicsUnit.Point);
                pageHeight = converter.ConvertToPixels(ldoc.Pages[0].Size.Height, PdfGraphicsUnit.Point);


                //Export the first page to the canvas.
                viewer = new PdfDocumentView();
                viewer.Load(ldoc);
                source = viewer.ExportAsImage(currentPageIndex);
                ImageBrush brush = new ImageBrush(source);
                diagramView1.SizeToContent = false;
                diagramView1.PageBackground = brush;
                diagramView1.BoundaryConstraintsArea = new Rect(0, 0, pageWidth, pageHeight);
            }
        }
      

        #endregion

    }
}
