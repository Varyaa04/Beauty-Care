using Aspose.BarCode.Generation;
using Beauty_Care.goods;
using Beauty_Care.goodsAdmin;
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

namespace Beauty_Care.cartPage
{
    /// <summary>
    /// Логика взаимодействия для qrPage.xaml
    /// </summary>
    public partial class qrPage : Page
    {
        public qrPage()
        {
            InitializeComponent();

            int qr = 1;

            BitmapImage bitmap = new BitmapImage();
            BarcodeGenerator gen = new BarcodeGenerator(EncodeTypes.QR, "https://masterpiecer-images.s3.yandex.net/5e8dde6f838711ee83aad20dae950626:upscaled");
            gen.Parameters.Barcode.XDimension.Pixels = 34;

            string dataDir = @"C:\Users\10210808\source\repos\Beauty-Care\";
            gen.Save(dataDir + qr.ToString() + "picture.png", BarCodeImageFormat.Png);

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(dataDir + qr.ToString() + "picture.png");
            bitmap.EndInit();

            img.Source = bitmap;
            qr++;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
