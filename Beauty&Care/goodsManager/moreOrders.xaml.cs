using Beauty_Care.goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

namespace Beauty_Care.goodsManager
{
    /// <summary>
    /// Логика взаимодействия для moreOrders.xaml
    /// </summary>
    public partial class moreOrders : Page
    {
        public moreOrders(ordersManager selectedorder)
        {
            var curentorderid = selectedorder.idOrderManager;
            var curentorderuser = selectedorder.orders.idUsers;
            var curentorderstatus = selectedorder.orders.idStatus;
            InitializeComponent();

            var moreorder = Entities.GetContext().ordersManager
                               .Where(m => m.idOrder == curentorderid)
                               .Select(m => m.idGoods)
                               .ToList();
            var goodsInorder = Entities.GetContext().beautyGoods
                                         .Where(x => moreorder.Contains(x.idGoods))
                                         .ToList();
            listgoodsorder.ItemsSource = goodsInorder;

            var userlogin = Entities.GetContext().users
                               .FirstOrDefault(s => s.idUser == curentorderuser);
            labeluser.Content = userlogin.login;

            labelId.Content = selectedorder.orders.idUsers;

            var statusorder = Entities.GetContext().status
                               .FirstOrDefault(s => s.idStatus == curentorderstatus);

            labelstatus.Content = statusorder.idStatus;
        }

        private void ToOrders_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new managerOrders());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }
    }
}