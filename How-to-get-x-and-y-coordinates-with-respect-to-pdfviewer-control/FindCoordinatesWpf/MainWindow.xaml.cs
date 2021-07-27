using System.Windows;
using System.Windows.Input;

namespace FindCoordinatesWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PdfViewer.Load("../../Data/GIS Succinctly.pdf");
            PdfViewer.MouseUp += PdfViewer_MouseUp;
        }

        private void PdfViewer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Drawing.Point position = new System.Drawing.Point
            {
                X = (int)e.GetPosition(PdfViewer).X,
                Y = (int)e.GetPosition(PdfViewer).Y
            };
            //Displays the x and y coordinates
            xLabel.Content = position.X;
            yLabel.Content = position.Y;
        }
    }
}
