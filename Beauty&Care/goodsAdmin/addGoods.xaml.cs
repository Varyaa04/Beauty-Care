using Beauty_Care.goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
    public partial class addGoods : Page
    {

        public addGoods()
        {
            InitializeComponent();

            ComboCategory.ItemsSource = Entities.GetContext().category.Select(x => x.nameCategory).ToList();
            ComboMan.ItemsSource = Entities.GetContext().manufacturer.Select(x => x.namemManufacturer).ToList();

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.modeldb.beautyGoods.Count(x => x.article == article.Text) > 0)
            {
                MessageBox.Show("Товар с таким артиклем существует!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (article.Text == "")
            {
                MessageBox.Show("Введите значение 'артикль'!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (instock.Text == "")
            {
                MessageBox.Show("Введите значение 'в наличии'!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (nameTB.Text == "")
            {
                MessageBox.Show("Введите наименование товара!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (compoundTB.Text == "")
            {
                MessageBox.Show("Введите состав продукта!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (price.Text == "")
            {
                MessageBox.Show("Введите значение 'Цена'!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                int stock = Convert.ToInt32(instock.Text);
                int cost = Convert.ToInt32(price.Text);
                try
                {
                    beautyGoods goodsobj = new beautyGoods()
                    {
                        article = article.Text,
                        nameGoods = nameTB.Text,
                        price = cost,
                        manufacturer = Convert.ToInt32(ComboMan.SelectedIndex + 1),
                        typeGoods = Convert.ToInt32(ComboType.SelectedIndex + 1),
                        category = Convert.ToInt32(ComboCategory.SelectedIndex + 1),
                        instock = stock,
                        compound = compoundTB.Text,
                        description = desc.Text,
                        image = null
                    }; 
                    AppConnect.modeldb.beautyGoods.Add(goodsobj);
                    AppConnect.modeldb.SaveChanges();
                    MessageBox.Show("Товар успешно добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppFrame.frameMain.GoBack();
                }
                catch
                {
                    MessageBox.Show("Ошибка при добавлении данных!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        private void BGoBackbutton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void ComboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ComboCategory.SelectedIndex == 0)
            {
                ComboType.ItemsSource = Entities.GetContext().typeGoods
                                           .Where(x => x.idType == 1 || x.idType == 2 || x.idType == 3)
                                           .Select(x => x.nameType)
                                           .ToList();
            }
            else if (ComboCategory.SelectedIndex == 1)
            {
                ComboType.ItemsSource = Entities.GetContext().typeGoods
                                           .Where(x => x.idType == 4 || x.idType == 5 || x.idType == 6)
                                           .Select(x => x.nameType)
                                           .ToList();
            }
            else if (ComboCategory.SelectedIndex == 2)
            {
                ComboType.ItemsSource = Entities.GetContext().typeGoods
                                           .Where(x => x.idType == 8 || x.idType == 7)
                                           .Select(x => x.nameType)
                                           .ToList();
            }
        }

        private void article_KeyDown(object sender, KeyEventArgs e)
        {

            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        private void instock_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        private void price_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        private void nameTB_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.A || e.Key > Key.Z) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }
    }
}
