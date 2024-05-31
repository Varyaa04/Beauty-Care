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

            ComboCategory.ItemsSource = Entities3.GetContext().category.Select(x => x.nameCategory).ToList();
            ComboMan.ItemsSource = Entities3.GetContext().manufacturer.Select(x => x.namemManufacturer).ToList();

            article.MaxLength = 16;
            nameTB.MaxLength = 30;
            instock.MaxLength = 3;
            price.MaxLength = 8;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool ValidateFields()
        {
            if (AppConnect.modeldb.beautyGoods.Any(x => x.article == article.Text))
            {
                ShowError("Товар с таким артиклем существует!");
                return false;
            }
            if (string.IsNullOrEmpty(article.Text))
            {
                ShowError("Введите значение 'артикль'!");
                return false;
            }
            if (string.IsNullOrEmpty(instock.Text))
            {
                ShowError("Введите значение 'в наличии'!");
                return false;
            }
            if (string.IsNullOrEmpty(nameTB.Text))
            {
                ShowError("Введите наименование товара!");
                return false;
            }
            if (string.IsNullOrEmpty(compoundTB.Text))
            {
                ShowError("Введите состав продукта!");
                return false;
            }
            if (string.IsNullOrEmpty(price.Text))
            {
                ShowError("Введите значение 'Цена'!");
                return false;
            }
            return true;
        }

        private beautyGoods CreateGoodsObject()
        {
            return new beautyGoods()
            {
                article = article.Text,
                nameGoods = nameTB.Text,
                price = Convert.ToInt32(price.Text),
                manufacturer = ComboMan.SelectedIndex + 1,
                typeGoods = ComboType.SelectedIndex + 1,
                category = ComboCategory.SelectedIndex + 1,
                instock = Convert.ToInt32(instock.Text),
                compound = compoundTB.Text,
                description = desc.Text,
                image = null
            };
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;
            beautyGoods goodsobj = CreateGoodsObject();
            AppConnect.modeldb.beautyGoods.Add(goodsobj);

            try
            {
                AppConnect.modeldb.SaveChanges();
                MessageBox.Show("Товар успешно добавлен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frameMain.GoBack();
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при добавлении данных: " + ex.Message);
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
                ComboType.ItemsSource = Entities3.GetContext().typeGoods
                                           .Where(x => x.idType == 1 || x.idType == 2 || x.idType == 3)
                                           .Select(x => x.nameType)
                                           .ToList();
            }
            else if (ComboCategory.SelectedIndex == 1)
            {
                ComboType.ItemsSource = Entities3.GetContext().typeGoods
                                           .Where(x => x.idType == 4 || x.idType == 5 || x.idType == 6)
                                           .Select(x => x.nameType)
                                           .ToList();
            }
            else if (ComboCategory.SelectedIndex == 2)
            {
                ComboType.ItemsSource = Entities3.GetContext().typeGoods
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

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                nameTB.Focus();
            }
        }

        private void instock_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                price.Focus();
            }
        }

        private void price_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                ComboType.Focus();
            }
        }

        private void nameTB_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.A || e.Key > Key.Z) && e.Key != Key.Back)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                ComboCategory.Focus();
            }
        }

        private void ComboCategory_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ComboType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                ComboMan.Focus();
            }
        }

        private void ComboMan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                compoundTB.Focus();
            }
        }

        private void compoundTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                desc.Focus();
            }
        }

        private void desc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                save.Focus();
            }
        }
    }
}
