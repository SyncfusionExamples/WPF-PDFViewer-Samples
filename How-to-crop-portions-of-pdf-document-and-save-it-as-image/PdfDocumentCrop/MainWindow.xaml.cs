using Microsoft.Win32;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InkAnnotate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        int currentPageIndex = 0;
        PdfDocumentView viewer;
        BitmapSource source;
        PdfLoadedDocument ldoc;
        PdfUnitConvertor converter = new PdfUnitConvertor();
        float pageWidth;
        float pageHeight;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private Rectangle currentRectangle;
        private Color rectangleColor;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
            LoadDocument("../../Data/Product Line Sales.pdf");
        }
        #endregion

        #region Events
        /// <summary>
        /// Modifies the brush color of the strokes by changing the drawing attributes of the canvas.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private void brushColor_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Modify the drawing attributes of the ink canvas.
            ColorEdit edit = d as ColorEdit;

            rectangleColor = edit.Color;
        }

        /// <summary>
        /// Saves the current strokes and navigates to the next page of the document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ldoc.Pages.Count > (currentPageIndex + 1))
            {
                SaveCroppedImages();

                foreach (var rect in rectangles)
                {
                    canvas1.Children.Remove(rect);
                }

                rectangles.Clear();

                //Save the current strokes and navigate to the next page.
                currentPageIndex++;
                source = viewer.ExportAsImage(currentPageIndex);
                ImageBrush brush = new ImageBrush(source);
                this.canvas1.Background = brush;
                this.currentPageBlock.Text = (currentPageIndex + 1).ToString();
            }
        }

        /// <summary>
        /// Saves the current strokes and navigates to the previous page of the document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            if ((currentPageIndex > 0) && (currentPageIndex <= ldoc.Pages.Count))
            {
                SaveCroppedImages();

                foreach (var rect in rectangles)
                {
                    canvas1.Children.Remove(rect);
                }

                rectangles.Clear();
                //Save the current strokes and navigate to the previous page.
                currentPageIndex--;
                source = viewer.ExportAsImage(currentPageIndex);
                ImageBrush brush = new ImageBrush(source);
                this.canvas1.Background = brush;
                this.currentPageBlock.Text = (currentPageIndex + 1).ToString();
            }

        }


        /// <summary>
        /// Opens the pdf document and displays the first page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            //Open a new pdf document
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".pdf";
            dialog.Filter = "Adobe Pdf Documents(.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                foreach (var rect in rectangles)
                {
                    canvas1.Children.Remove(rect);
                }

                rectangles.Clear();
                LoadDocument(dialog.FileName);
            }
        }

        /// <summary>
        /// Saves the annotated document and disposes the document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cropButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCroppedImages();
        }

        /// <summary>
        /// Disposes the document and closes the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            //Dispose the document and close the application.
            if (ldoc != null)
                ldoc.Close(true);
            this.Close();
        }

        /// <summary>
        /// Used to check the hit on the rectangle or on the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas1_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var rect in rectangles)
            {
                MouseStrikeType = SetStrikeType(rect,
        Mouse.GetPosition(canvas1));
                if (MouseStrikeType != StrikeType.None)
                {
                    currentRectangle = rect;
                    break;
                }
            }

            SetExpectedCursor();
            if (MouseStrikeType == StrikeType.None)
                return;

            LastDragPoint = Mouse.GetPosition(canvas1);
            isDragInProgress = true;
        }

        /// <summary>
        /// If clicked on rectangle this event is used to move the rectangle over the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas1_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragInProgress)
            {
                Point point = Mouse.GetPosition(canvas1);
                double offsetX = point.X - LastDragPoint.X;
                double offsetY = point.Y - LastDragPoint.Y;

                double newX = Canvas.GetLeft(currentRectangle);
                double newY = Canvas.GetTop(currentRectangle);
                double newWidth = currentRectangle.Width;
                double newHeight = currentRectangle.Height;

                switch (MouseStrikeType)
                {
                    case StrikeType.Body:
                        newX += offsetX;
                        newY += offsetY;
                        break;
                    case StrikeType.ULStrike:
                        newX += offsetX;
                        newY += offsetY;
                        newWidth -= offsetX;
                        newHeight -= offsetY;
                        break;
                    case StrikeType.URStrike:
                        newY += offsetY;
                        newWidth += offsetX;
                        newHeight -= offsetY;
                        break;
                    case StrikeType.LRStrike:
                        newWidth += offsetX;
                        newHeight += offsetY;
                        break;
                    case StrikeType.LLStrike:
                        newX += offsetX;
                        newWidth -= offsetX;
                        newHeight += offsetY;
                        break;
                    case StrikeType.LeftStrike:
                        newX += offsetX;
                        newWidth -= offsetX;
                        break;
                    case StrikeType.RightStrike:
                        newWidth += offsetX;
                        break;
                    case StrikeType.BottomStrike:
                        newHeight += offsetY;
                        break;
                    case StrikeType.TopStrike:
                        newY += offsetY;
                        newHeight -= offsetY;
                        break;
                }


                if ((newWidth > 0) && (newHeight > 0))
                {
                    Canvas.SetLeft(currentRectangle, newX);
                    Canvas.SetTop(currentRectangle, newY);
                    currentRectangle.Width = newWidth;
                    currentRectangle.Height = newHeight;

                    LastDragPoint = point;
                }
            }
            else
            {
                foreach (var rect in rectangles)
                {
                    MouseStrikeType = SetStrikeType(rect,
                        Mouse.GetPosition(canvas1));
                    if (MouseStrikeType != StrikeType.None)
                        break;
                }
                SetExpectedCursor();
            }
        }

        /// <summary>
        /// Used to set the flag that drag of rectangle is completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas1_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragInProgress = false;
        }

        /// <summary>
        /// Used to include rectangle in desired color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnIncludeRect_OnClick(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new Rectangle();
            Canvas.SetLeft(rectangle, 0);
            Canvas.SetTop(rectangle, 0);
            rectangle.Width = 100;
            rectangle.Height = 100;
            rectangle.Fill = new SolidColorBrush(rectangleColor);

            canvas1.Children.Add(rectangle);

            rectangles.Add(rectangle);
        }
        #endregion

        #region Methods
        private void SaveCroppedImages()
        {
            foreach (var rect in rectangles)
            {
                double left = Canvas.GetLeft(rect), top = Canvas.GetTop(rect);
                double width = rect.Width, height = rect.Height;

                BitmapSource croppedBitmap = new CroppedBitmap(source, new Int32Rect((int)left, (int)top, (int)width, (int)height));
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(croppedBitmap));
                String imageName = Guid.NewGuid().ToString() + ".png";
                FileStream stream = new FileStream("../../Output/" + imageName, FileMode.Create);
                encoder.Save(stream);
            }
        }

        /// <summary>
        /// Loads the Pdf document and displays the first page in the canvas.
        /// </summary>
        /// <param name="fileName"> File name of the Pdf document</param>
        private void LoadDocument(string fileName)
        {
            try
            {
                //Load the pdf document
                ldoc = new PdfLoadedDocument(fileName);

                //Convert page units to pixels and create drawing surface.
                pageWidth = converter.ConvertToPixels(ldoc.Pages[0].Size.Width, PdfGraphicsUnit.Point);
                pageHeight = converter.ConvertToPixels(ldoc.Pages[0].Size.Height, PdfGraphicsUnit.Point);
                this.canvas1.Height = pageHeight;
                this.canvas1.Width = pageWidth;

                //Export the first page to the canvas.
                viewer = new PdfDocumentView();
                viewer.RenderingEngine = PdfRenderingEngine.SfPdf;
                viewer.Load(ldoc);
                this.toatlPageBlock.Text = ldoc.Pages.Count.ToString();
                this.currentPageBlock.Text = (currentPageIndex + 1).ToString();
                source = viewer.ExportAsImage(currentPageIndex);
                ImageBrush brush = new ImageBrush(source);
                this.canvas1.Background = brush;
            }
            catch
            {
                MessageBox.Show("An error occured while loading the document. Please contact Syncfusion support", "An Error Occured!!");
            }

        }

        #endregion

        #region Helper

        private enum StrikeType
        {
            None, Body, ULStrike, URStrike, LRStrike, LLStrike, LeftStrike, RightStrike, TopStrike, BottomStrike
        };


        StrikeType MouseStrikeType = StrikeType.None;


        private bool isDragInProgress = false;


        private Point LastDragPoint;



        private StrikeType SetStrikeType(Rectangle rect, Point point)
        {
            double canvasLeft = Canvas.GetLeft(rect);
            double canvasTop = Canvas.GetTop(rect);
            double canvasRight = canvasLeft + rect.Width;
            double canvasBottom = canvasTop + rect.Height;
            if (point.X < canvasLeft) return StrikeType.None;
            if (point.X > canvasRight) return StrikeType.None;
            if (point.Y < canvasTop) return StrikeType.None;
            if (point.Y > canvasBottom) return StrikeType.None;

            const double gap = 10;
            if (point.X - canvasLeft < gap)
            {

                if (point.Y - canvasTop < gap) return StrikeType.ULStrike;
                if (canvasBottom - point.Y < gap) return StrikeType.LLStrike;
                return StrikeType.LeftStrike;
            }
            else if (canvasRight - point.X < gap)
            {

                if (point.Y - canvasTop < gap) return StrikeType.URStrike;
                if (canvasBottom - point.Y < gap) return StrikeType.LRStrike;
                return StrikeType.RightStrike;
            }
            if (point.Y - canvasTop < gap) return StrikeType.TopStrike;
            if (canvasBottom - point.Y < gap) return StrikeType.BottomStrike;
            return StrikeType.Body;
        }

        private void SetExpectedCursor()
        {
            Cursor expectedCursor = Cursors.Arrow;
            switch (MouseStrikeType)
            {
                case StrikeType.None:
                    expectedCursor = Cursors.Arrow;
                    break;
                case StrikeType.Body:
                    expectedCursor = Cursors.ScrollAll;
                    break;
                case StrikeType.ULStrike:
                case StrikeType.LRStrike:
                    expectedCursor = Cursors.SizeNWSE;
                    break;
                case StrikeType.LLStrike:
                case StrikeType.URStrike:
                    expectedCursor = Cursors.SizeNESW;
                    break;
                case StrikeType.TopStrike:
                case StrikeType.BottomStrike:
                    expectedCursor = Cursors.SizeNS;
                    break;
                case StrikeType.LeftStrike:
                case StrikeType.RightStrike:
                    expectedCursor = Cursors.SizeWE;
                    break;
            }
            if (Cursor != expectedCursor) Cursor = expectedCursor;
        }


        #endregion
    }
}
