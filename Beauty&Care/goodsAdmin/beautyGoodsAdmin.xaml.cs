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

namespace Beauty_Care.goodsAdmin
{
    public partial class beautyGoodsAdmin : Page
    {
        public beautyGoodsAdmin()
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

            comboFilter.ItemsSource = Entities.GetContext().category.Select(x => x.nameCategory).ToList();
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
            var goodsDel = ListGoods.SelectedItems.Cast<beautyGoods>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {goodsDel.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var context = Entities.GetContext();
                    foreach (var item in goodsDel)
                    {
                        var beautyGood = context.beautyGoods.Find(item.idGoods);
                        if (beautyGood != null)
                        {
                            context.beautyGoods.Remove(beautyGood);
                        }
                    }
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

                    context.SaveChanges();
                    MessageBox.Show("Данные удалены");
                    ListGoods.SelectedItem = null;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
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
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ListGoods.ItemsSource = Entities.GetContext().beautyGoods.ToList();
            }
        }
    }
}