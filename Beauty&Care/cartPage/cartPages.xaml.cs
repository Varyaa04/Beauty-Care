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
        private orders userRole = new orders();
        private users Useriddd = new users();
        public cartPages()
        {
            InitializeComponent();
            //int userCart = Convert.ToInt32(App.Current.Properties["idUser"] = userRole.idUsers);

            //List<orders> order = AppConnect.modeldb.orders.ToList();
            //ListOrders.ItemsSource = Entities.GetContext().orders
            //                               .Where(x => x.idUsers == userRole.idUsers)
            //                               .Select(x => x.idGoods)
            //                               .ToList();
            ////order = order.Where(x => x.idUsers == 1).Select(x => x.idGoods).ToList();

            //if (order.Count > 0)
            //{
            //    tbCounter.Text = "Всего в корзине " + order.Count + " товаров";
            //}
            //else
            //{
            //    tbCounter.Text = "Ваша корзина пуста!";
            //}

            int userCart = Convert.ToInt32(App.Current.Properties["idUser"] = Useriddd.idUser);

            List<orders> order = AppConnect.modeldb.orders.ToList();
            ListOrders.ItemsSource = Entities.GetContext().orders
                                           .Where(x => x.idUsers == Useriddd.idUser)
                                           .Select(x => x.idGoods)
                                           .ToList();
            //order = order.Where(x => x.idUsers == 1).Select(x => x.idGoods).ToList();

            if (order.Count > 0)
            {
                tbCounter.Text = "Всего в корзине " + order.Count + " товаров";
            }
            else
            {
                tbCounter.Text = "Ваша корзина пуста!";
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void Page_visible(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ListOrders.ItemsSource = Entities.GetContext().beautyGoods.ToList();
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
