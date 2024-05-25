using Beauty_Care.goods;
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

        public moreOrders(ordersManager selectedOrders)
        {
            InitializeComponent();

            if (selectedOrders != null)
            {
                _currentOrders = selectedOrders;
            }
            DataContext = _currentOrders;

            AppConnect.modeldb.ordersManager.ToArray();


            comboStatus.ItemsSource = Entities.GetContext().status.Select(x => x.nameStatus).ToList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrders.idOrder == 0)
            {
                Entities.GetContext().ordersManager.Add(_currentOrders);
            }
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно изменены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
}
