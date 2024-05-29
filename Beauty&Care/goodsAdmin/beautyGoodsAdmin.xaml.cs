using Beauty_Care.goods;
using Beauty_Care.auth;
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
using Beauty_Care.goodsAdmin.usersPages;
using System.Windows.Shapes;

namespace Beauty_Care.goodsAdmin
{
    public partial class beautyGoodsAdmin : Page
    {
        private users _authAdmin = new users();
        private orders order = new orders();
        public beautyGoodsAdmin(users authUser)
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

            comboFilter.ItemsSource = Entities3.GetContext().category.Select(x => x.nameCategory).ToList();

            if(authUser != null)
            {
                _authAdmin = authUser;
            }
            DataContext = _authAdmin;
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


        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить выбранный товар?", "Подтверждение удаления",
               MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ListGoods.ItemsSource = Entities3.GetContext().beautyGoods.ToList();
                Button b = sender as Button;
                int ID = int.Parse(((b.Parent as StackPanel).Children[0] as TextBlock).Text);
                Console.WriteLine(ID);
                AppConnect.modeldb.beautyGoods.Remove(
                    AppConnect.modeldb.beautyGoods.Where(x => x.idGoods == ID).First());
                AppConnect.modeldb.SaveChanges();
                AppFrame.frameMain.Navigate(new beautyGoodsAdmin((sender as Button).DataContext as users));
            }

        }
        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new addGoods());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new editGoods((sender as Button).DataContext as beautyGoods));
        }

        private void pageVisible(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities3.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ListGoods.ItemsSource = Entities3.GetContext().beautyGoods.ToList();
            }
        }

        private void addBasket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListGoods.ItemsSource = Entities3.GetContext().beautyGoods.ToList();
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

        private void btnMore(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new singleItem((sender as Button).DataContext as beautyGoods));
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new cartPages());

        }

        private void editUser_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new UsersPage());

        }

        private void textboxSearch_TextChanged(object sender, TextChangedEventArgs e)
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

        private void buttonResetSearch_Click(object sender, RoutedEventArgs e)
        {

            textboxSearch.Text = string.Empty;
            findGoods();
        }

        private void buttonResetFilter_Click(object sender, RoutedEventArgs e)
        {
            comboFilter.SelectedIndex = -1;
            findGoods();
        }

        private void buttonResetSort_Click(object sender, RoutedEventArgs e)
        {
            comboSort.SelectedIndex = -1;
            findGoods();
        }

        private void btnEx_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти в главное меню?", "Подтверждение",
                          MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AppFrame.frameMain.Navigate(new sign_in());
            }
        }
    }
}