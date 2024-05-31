using Beauty_Care.cartPage;
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

namespace Beauty_Care.goods
{
    /// <summary>
    /// Логика взаимодействия для singleItem.xaml
    /// </summary>
    public partial class singleItem : Page
    {
        private beautyGoods _currentGoods = new beautyGoods();
        public singleItem(beautyGoods selectedGoods)
        {
            InitializeComponent();
            
            if (selectedGoods != null)
            {
                _currentGoods = selectedGoods;
            }
            DataContext = _currentGoods;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void addBasket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                int ID = int.Parse(((b.Parent as StackPanel).Children[0] as TextBlock).Text);
                int idUsers = Convert.ToInt32(App.Current.Properties["idUser"].ToString());

                int selectedGoodsId = ID;
                var order = Entities3.GetContext().orders.FirstOrDefault(o => o.idUsers == idUsers);
                if (order == null)
                {
                    order = new orders()
                    {
                        idUsers = idUsers,
                        idStatus = 1
                    };
                    Entities3.GetContext().orders.Add(order);
                    Entities3.GetContext().SaveChanges();
                }
                var cartnew = new cart()
                {
                    idOrder = order.idOrder,
                    idGoods = selectedGoodsId
                };

                Entities3.GetContext().cart.Add(cartnew);
                Entities3.GetContext().SaveChanges();


                MessageBox.Show("Товар успешно добавлен в корзину!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frameMain.Navigate(new cartPages());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении товара в корзину: " + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
