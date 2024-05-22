using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beauty_Care.goodsAdmin;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Beauty_Care.goods;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aspose.BarCode.Generation;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = iTextSharp.text.Image;
using Paragraph = iTextSharp.text.Paragraph;
using System.IO;

namespace Beauty_Care.cartPage
{
    /// <summary>
    /// Логика взаимодействия для cartPages.xaml
    /// </summary>
    public partial class cartPages : Page
    {
        public cartPages()
        {
            InitializeComponent();

            int idusercart = Convert.ToInt32(App.Current.Properties["idUser"].ToString());



            var goodsobj = Entities.GetContext().orders
                                        .Where(x => x.idUsers == idusercart)
                                        .Select(x => x.idGoods)
                                        .ToList();
            var listgoods = Entities.GetContext().beautyGoods.Where(x => goodsobj.Contains(x.idGoods)).ToList();


            ListOrders.ItemsSource = listgoods;

            if (ListOrders.Items.Count > 0)
            {
                tbCounter.Text = "Всего в корзине " + ListOrders.Items.Count + " товаров";
            }
            else
            {
                tbCounter.Text = "Ваша корзина пуста!";
            }


        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var userObj = AppConnect.modeldb.users;
            int us = Convert.ToInt32(App.Current.Properties["roleUser"].ToString());
            if (us == 1)
            {
                AppFrame.frameMain.Navigate(new beautyGoodsAdmin((sender as Button).DataContext as users));
            }

            else if (us == 2)
            {
                AppFrame.frameMain.Navigate(new beautyGoodsPages((sender as Button).DataContext as users));
            }

        }



        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            int idusercart = Convert.ToInt32(App.Current.Properties["idUser"].ToString());

            try
            {
                var goodsDel = ListOrders.SelectedItems.Cast<orders>().Where(x => x.idUsers == idusercart).ToList();

                if (MessageBox.Show($"Вы точно хотите удалить следующие {goodsDel.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = Entities.GetContext();
                        foreach (var item in goodsDel)
                        {
                            var beautyGood = context.orders.Find(item.idGoods);
                            if (beautyGood != null)
                            {
                                context.orders.Remove(beautyGood);
                            }
                        }
                        var goods = Entities.GetContext().orders.ToList();
                        if (goods.Count > 0)
                        {
                            tbCounter.Text = "Найдено " + goods.Count + " товаров";
                        }
                        else
                        {
                            tbCounter.Text = "Ничего не найдено";
                        }
                        ListOrders.ItemsSource = (goods);
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены");
                        ListOrders.SelectedItem = null;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ListOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            CreatePDF();
            MessageBox.Show("PDF документ был успешно загружен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            AppFrame.frameMain.Navigate(new qrPage());

        }

        private void CreatePDF()
        {
            Document document = new Document();

            try
            {
                PdfWriter.GetInstance(document, new FileStream("..\\..\\order.pdf", FileMode.Create));

                document.Open();
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font font = new Font(baseFont, 14);
                Font font1 = new Font(baseFont, 24, 2, BaseColor.GRAY);
                Paragraph paragraph1 = new Paragraph("Список товаров", font1);
                paragraph1.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph1);
                decimal sum = 0;

                foreach (var item in AppConnect.modeldb.orders.ToList())
                {
                    if (item is orders)
                    {
                        orders data = (orders)item;

                        Image img = Image.GetInstance("C:\\Users\\10210808\\source\\repos\\Beauty-Care\\Beauty&Care\\" + data.beautyGoods.CurrentPhoto);
                        img.ScaleAbsolute(100f, 100f);
                        document.Add(img);
                        document.Add(new Paragraph("Название: " + data.beautyGoods.nameGoods, font));
                        document.Add(new Paragraph("Категория: " + data.beautyGoods.category1.nameCategory, font));
                        document.Add(new Paragraph("Тип товара: " + data.beautyGoods.typeGoods1.nameType, font));
                        document.Add(new Paragraph("Описание: " + data.beautyGoods.description, font));
                        document.Add(new Paragraph("Цена: " + data.beautyGoods.price.ToString() + " руб.", font));

                        sum += data.beautyGoods.price;

                    }
                }
                Paragraph paragraph = new Paragraph("Сумма = " + sum.ToString(), font);
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);
            }
            catch (DocumentException de)
            {
                MessageBox.Show(de.Message);
            }
            catch (IOException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
            finally
            {
                document.Close();
            }
        }
    }
}