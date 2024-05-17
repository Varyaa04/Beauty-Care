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
    /// <summary>
    /// Логика взаимодействия для editGoods.xaml
    /// </summary>
    public partial class editGoods : Page
    {
        private beautyGoods _currentGoods = new beautyGoods();
        public editGoods(beautyGoods selectedGoods)
        {
            InitializeComponent();

            if (selectedGoods != null)
            {
                _currentGoods = selectedGoods;
            }
            DataContext = _currentGoods;

            ComboCategory.ItemsSource = Entities.GetContext().category.Select(x => x.nameCategory).ToList();
            ComboType.ItemsSource = Entities.GetContext().typeGoods.Select(x => x.nameType).ToList();
            ComboMan.ItemsSource = Entities.GetContext().manufacturer.Select(x => x.namemManufacturer).ToList();
        }

        private void BGoBackbutton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(_currentGoods.article))
            {
                errors.AppendLine("Введите артикль");
            }
            else if (instock.Text == "")
            {
                MessageBox.Show("Введите значение 'в наличии'!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
            if (nameTB.Text == "")
            {
                MessageBox.Show("Введите наименование товара!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
            else if (compoundTB.Text == "")
            {
                MessageBox.Show("Введите состав продукта!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
            else if (price.Text == "")
            {
                MessageBox.Show("Введите значение 'Цена'!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
            else
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentGoods.idGoods == 0)
            {
                Entities.GetContext().beautyGoods.Add(_currentGoods);
            }
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно изменены!");
                AppFrame.frameMain.Navigate(new beautyGoodsAdmin());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
    }
}
