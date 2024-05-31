using Beauty_Care.goods;
using Org.BouncyCastle.Asn1.X509;
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

namespace Beauty_Care.goodsManager
{
    /// <summary>
    /// Логика взаимодействия для moreOrders.xaml
    /// </summary>
    public partial class moreOrders : Page
    {
        private ordersManager _currentOrders = new ordersManager();
        public moreOrders(ordersManager selectedorder)
        {
            InitializeComponent();
            if (selectedorder != null)
            {
                _currentOrders = selectedorder;
                DataContext = _currentOrders;

                var currentOrderId = _currentOrders.idOrder;

                var goodsInOrder = Entities3.GetContext().beautyGoods
                    .Where(bg => bg.ordersManager.Any(om => om.idOrder == currentOrderId))
                    .ToList();
                listgoodsorder.ItemsSource = goodsInOrder;

                var user = Entities3.GetContext().users
                    .FirstOrDefault(u => u.orders.Any(o => o.idOrder == currentOrderId));

                labeluser.Content = user?.nameUser;
                labelName.Content = user?.nameUser;
                labelPhone.Content = user?.phone;
                labelEmai.Content = user?.email;

                var statusOrder = Entities3.GetContext().status
                    .FirstOrDefault(s => s.orders.Any(o => o.idOrder == currentOrderId));
                comboStatus.ItemsSource = Entities3.GetContext().status.ToList();
                comboStatus.SelectedValue = statusOrder?.idStatus;
            }
            else
            {
                _currentOrders = new ordersManager();
                DataContext = _currentOrders;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите поменять статус заказа?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    int Id = _currentOrders.idOrder;
                    var StatusToUpdate = AppConnect.modeldb.ordersManager.FirstOrDefault(u => u.idOrder == Id);
                    if (comboStatus.SelectedItem != null)
                    {
                        StatusToUpdate.orders.idStatus = Convert.ToInt32(comboStatus.SelectedIndex + 1);

                        AppConnect.modeldb.SaveChanges();
                        MessageBox.Show("Статус заказа успешно изменен!",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
    }
}