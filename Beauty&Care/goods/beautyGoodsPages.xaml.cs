using Beauty_Care.cartPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для beautyGoods.xaml
    /// </summary>
    public partial class beautyGoodsPages : Page
    {
        private users _authOrd = new users();
        private orders order = new orders();
        public beautyGoodsPages(users authUser)
        {
            InitializeComponent();
            List<beautyGoods> goods = AppConnect.modeldb.beautyGoods.ToList();

            if (goods.Count > 0)
            {
                tbCounter.Text = "Найдено " + goods.Count + " товаров";
            }
            else
            {
                tbCounter.Text = "Ничего не найдено";
            }
            ListGoods.ItemsSource = goods;

            comboSort.Items.Add("По возрастанию цены");
            comboSort.Items.Add("По убыванию цены");
            comboSort.Items.Add("По названию от А-Я");
            comboSort.Items.Add("По названию от Я-А");

            comboFilter.ItemsSource = Entities.GetContext().category.Select(x =>  x.nameCategory).ToList();

            if( authUser != null)
            {
                _authOrd = authUser;
            }
            DataContext = _authOrd;
        }


        beautyGoods[] findGoods()
        {

            List<beautyGoods> products = AppConnect.modeldb.beautyGoods.ToList();

            if (textboxSearch != null)
            {
                products = products.Where(x => x.nameGoods.ToLower().Contains(textboxSearch.Text.ToLower())).ToList();
            }

            if (comboFilter.SelectedIndex >= 0)
            {
                switch (comboFilter.SelectedIndex)
                {
                    case 0:
                        products = products.Where(x => x.category == 1).ToList();
                        break;
                    case 1:
                        products = products.Where(x => x.category == 2).ToList();
                        break;
                    case 2:
                        products = products.Where(x => x.category == 3).ToList();
                        break;
                }
            }


            if (comboSort.SelectedIndex >= 0)
            {
                switch (comboSort.SelectedIndex)
                {
                    case 0:
                        products = products.OrderBy(x => x.price).ToList();
                        break;
                    case 1:
                        products = products.OrderByDescending(x => x.price).ToList();
                        break;
                    case 2:
                        products = products.OrderBy(x => x.nameGoods).ToList();
                        break;
                    case 3:
                        products = products.OrderByDescending(x => x.nameGoods).ToList();
                        break;
                }
            }
            if (products.Count > 0)
            {
                tbCounter.Text = "Найдено " + products.Count + " товаров";
            }
            else
            {
                tbCounter.Text = "Ничего не найдено";
            }
            ListGoods.ItemsSource = products;
            return products.ToArray();

        }
       
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            findGoods();
        }

        private void comboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            findGoods();
        }


        private void comboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            findGoods();
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            textboxSearch.Text = string.Empty;
            comboFilter.SelectedIndex = -1;
            comboSort.SelectedIndex = -1;
            findGoods();
        }

        private void addBasket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListGoods.ItemsSource = Entities.GetContext().beautyGoods.ToList();
                Button b = sender as Button;
                int ID = int.Parse(((b.Parent as StackPanel).Children[0] as TextBlock).Text);
                int idUsers = Convert.ToInt32(App.Current.Properties["idUser"].ToString());

                int selectedGoodsId = ID;
                var order = Entities.GetContext().orders.FirstOrDefault(o => o.idUsers == idUsers);
                if (order == null)
                {
                    order = new orders()
                    {
                        idUsers = idUsers,
                        idStatus = 1
                    };
                    Entities.GetContext().orders.Add(order);
                    Entities.GetContext().SaveChanges();
                }

                var cartnew = new cart()
                {
                    idOrder = order.idOrder,
                    idGoods = selectedGoodsId
                };

                Entities.GetContext().cart.Add(cartnew);
                Entities.GetContext().SaveChanges();


                MessageBox.Show("Товар успешно добавлен в корзину!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frameMain.Navigate(new cartPages());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении товара в корзину: " + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnMore(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new singleItem((sender as Button).DataContext as beautyGoods));
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new cartPages());
        }
    }
}
