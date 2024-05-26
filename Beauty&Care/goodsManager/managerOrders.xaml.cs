﻿using System;
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
using Beauty_Care.goods;

namespace Beauty_Care.goodsManager
{
    /// <summary>
    /// Логика взаимодействия для managerOrders.xaml
    /// </summary>
    public partial class managerOrders : Page
    {
        public managerOrders()
        {
            InitializeComponent();

            ListOrders.ItemsSource = AppConnect.modeldb.ordersManager.GroupBy(x => x.orders.idUsers).ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new auth.sign_in());
        }

        private void btnMore(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new moreOrders((sender as Button).DataContext as ordersManager));
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить выбранный заказ?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                ListOrders.ItemsSource = Entities.GetContext().ordersManager.ToList();
                Button b = sender as Button;
                int ID = int.Parse(((b.Parent as StackPanel).Children[0] as TextBlock).Text);
                int IDOrder = int.Parse(((b.Parent as StackPanel).Children[1] as TextBlock).Text);
                AppConnect.modeldb.ordersManager.Remove(
                    AppConnect.modeldb.ordersManager.Where(x => x.idOrderManager == ID).First());

                AppConnect.modeldb.orders.Remove(
                    AppConnect.modeldb.orders.Where(x => x.idOrder == IDOrder).First());
                AppConnect.modeldb.SaveChanges();
                AppFrame.frameMain.GoBack();
                AppFrame.frameMain.Navigate(new managerOrders());
            }
        }

        private void btnCatalog_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new beautyGoodsPages((sender as Button).DataContext as users));
        }
    }
}
