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


            ListOrders.ItemsSource = Entities.GetContext().orders
                                        .Where(x => x.idUsers == idusercart)
                                        .Select(x => x.idGoods)
                                        .ToList();
            List<orders> goods = AppConnect.modeldb.orders.ToList();

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
            if(us == 1)
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
            var goodsDel = ListOrders.SelectedItems.Cast<orders>().ToList();

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

        private void ListOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
