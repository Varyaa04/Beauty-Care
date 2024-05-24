﻿using System;
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
        int idusercart = Convert.ToInt32(App.Current.Properties["idUser"].ToString());

        public cartPages()
        {
            InitializeComponent();
            var goodsobj = Entities.GetContext().cart
                                        .Where(x => x.orders.idUsers == idusercart)
                                        .ToList();

            ListOrders.ItemsSource = goodsobj;

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
            if (MessageBox.Show("Вы точно хотите удалить выбранный товар из заказа?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                ListOrders.ItemsSource = Entities.GetContext().cart.ToList();
                Button b = sender as Button;
                int ID = int.Parse(((b.Parent as StackPanel).Children[0] as TextBlock).Text);
                Console.WriteLine(ID);
                AppConnect.modeldb.cart.Remove(
                    AppConnect.modeldb.cart.Where(x => x.idCart == ID).First() );
                AppConnect.modeldb.SaveChanges();
                AppFrame.frameMain.GoBack();
                AppFrame.frameMain.Navigate(new cartPages());
            }
        }

       
        private void ListOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы точно хотите оформить заказ? Все товары из вашей корзины будут удалены!", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    addManager();

                    CreatePDF();
                    MessageBox.Show("PDF документ был успешно загружен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);


                    RemoveItemsFromCart();
                    AppFrame.frameMain.Navigate(new qrPage());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
             
            }


        }

        private void CreatePDF()
        {
            Document document = new Document();

            try
            {
                string fileName = System.IO.Path.Combine("C:\\Users\\10210808\\Downloads", $"order_{DateTime.Now:yyyyMMddHHss}.pdf");
                PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
                document.Open();
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font font = new Font(baseFont, 12);
                Font font1 = new Font(baseFont, 24, 2, BaseColor.GRAY);
                Paragraph paragraph1 = new Paragraph("Список товаров", font1);
                paragraph1.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph1);
                decimal sum = 0;

                var goodsobj = Entities.GetContext().cart
                                     .Where(x => x.orders.idUsers == idusercart)
                                     .ToList();
                foreach (var item in goodsobj)
                {
                    if (item is cart data)
                    {
                        Image img = Image.GetInstance("C:\\Users\\10210808\\Desktop\\q\\Beauty&Care\\" + data.beautyGoods.CurrentPhoto);
                        img.ScaleAbsolute(100f, 100f);
                        document.Add(img);
                        document.Add(new Paragraph("Название: " + data.beautyGoods.nameGoods, font));
                        document.Add(new Paragraph("Категория: " + data.beautyGoods.category1.nameCategory, font));
                        document.Add(new Paragraph("Тип товара: " + data.beautyGoods.typeGoods1.nameType, font));
                        document.Add(new Paragraph("Описание: " + data.beautyGoods.description, font));
                        document.Add(new Paragraph("Цена: " + data.beautyGoods.price.ToString() + " руб.", font));
                        document.Add(new Paragraph(" "));
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
        private void pageVisible(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ListOrders.ItemsSource = Entities.GetContext().cart.ToList();
            }
        }

        public void RemoveItemsFromCart()
        {
            int idusercart = Convert.ToInt32(App.Current.Properties["idUser"].ToString());
            //var context = Entities.GetContext();

            //var goodsIds = goodsfordeleting.Select(x => x.idOrder).ToList();

            //var ordersToRemove = context.cart
            //                           .Where(x => x.orders.idUsers == idusercart && goodsIds.Contains(x.beautyGoods.idGoods))
            //                           .ToList();

            //context.cart.RemoveRange(ordersToRemove);

            //context.SaveChanges();

            //var remainingGoodsInCart = context.beautyGoods
            //                                  .Where(x => context.cart.Any(o => o.orders.idUsers == idusercart && o.idGoods == x.idGoods))
            //                                  .ToList();

            //ListOrders.ItemsSource = remainingGoodsInCart;

            var orderId = Entities.GetContext().orders.Where(x => x.idUsers == idusercart && x.idStatus == 1).ToList();
            List<cart> ordersRemove = AppConnect.modeldb.cart.ToList();
            ordersRemove.Select(x => x.idOrder == x.orders.idOrder).ToList();
            foreach (cart item in ordersRemove)
            {
                Entities.GetContext().cart.Remove(item);
                Entities.GetContext().SaveChanges();
            }
        
        }

        public void addManager()
        {
            int idUsers = Convert.ToInt32(App.Current.Properties["idUser"].ToString());
            var order = Entities.GetContext().orders.FirstOrDefault(o => o.idUsers == idUsers);
            var cartt = Entities.GetContext().cart.FirstOrDefault(o => o.orders.idUsers == idUsers);
            List<ordersManager> orders = AppConnect.modeldb.ordersManager.ToList();
            orders.Where(x => x.orders.idUsers == idUsers).ToList();
            foreach (var item in orders)
            {
                var cartnew = new ordersManager()
                {
                    idOrder = order.idOrder,
                    idGoods = cartt.idGoods
                };
                Entities.GetContext().ordersManager.Add(cartnew);
                Entities.GetContext().SaveChanges();
            }
            

           
        }
    }
}