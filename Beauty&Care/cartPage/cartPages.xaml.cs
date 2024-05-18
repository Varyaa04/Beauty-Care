using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            List<orders> goods = AppConnect.modeldb.orders.ToList();

            if (goods.Count > 0)
            {
                tbCounter.Text = "Всего в корзине " + goods.Count + " товаров";
            }
            else
            {
                tbCounter.Text = "Ваша корзина пуста!";
            }
            ListGoods.ItemsSource = goods;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }
    }
}
