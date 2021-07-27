using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Shapes;

namespace GettingStarted_2008
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //create an instance for PdfLoadedDocument
            PdfLoadedDocument ldoc = new PdfLoadedDocument("../../Data/Manual.pdf");
            //ExportAsImage method returns specified page in the PDF document as Bitmap image 
            Bitmap[] images = ldoc.ExportAsImage(0, ldoc.Pages.Count - 1);
            //Tiff conversion
            ImageCodecInfo encoderInfo = GetEncoderInfo("image/tiff");
            //Initialize EncoderParameters that contain specified number of Encoderparameter objects
            EncoderParameters encoderParams = new EncoderParameters(2);
            //Initialize EncoderParameter
            EncoderParameter parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionNone);
            encoderParams.Param[0] = parameter;
            parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
            encoderParams.Param[1] = parameter;
            System.Drawing.Image tiff = null;
            for (int i = 0; i < images.Length; i++)
            {
                if (i == 0)
                {
                    tiff = images[i];
                    //Save the tiff image into local disk
                    tiff.Save("../../Output/output.tiff", encoderInfo, encoderParams);
                }
                else
                {
                    System.Drawing.Image image = images[i];
                    parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                    encoderParams.Param[1] = parameter;
                    //Add the subsequent image to save into the local disk
                    tiff.SaveAdd(image, encoderParams);

                }
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}

