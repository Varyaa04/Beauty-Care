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

            ComboCategory.ItemsSource = Entities.GetContext().category.ToList();
            ComboMan.ItemsSource = Entities.GetContext().manufacturer.ToList();

            article.MaxLength = 16;
            nameTB.MaxLength = 30;
            instock.MaxLength = 3;
            price.MaxLength = 8;
        }

        private void BGoBackbutton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool ValidateFields()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(_currentGoods.article))
                errors.AppendLine("Введите артикль.");
            if (string.IsNullOrEmpty(instock.Text))
                errors.AppendLine("Введите значение 'в наличии'.");
            if (string.IsNullOrEmpty(nameTB.Text))
                errors.AppendLine("Введите наименование товара.");
            if (string.IsNullOrEmpty(compoundTB.Text))
                errors.AppendLine("Введите состав продукта.");
            if (string.IsNullOrEmpty(price.Text))
                errors.AppendLine("Введите значение 'Цена'.");
            if (ComboCategory.SelectedIndex == -1)
                errors.AppendLine("Выберите категорию.");
            if (ComboMan.SelectedIndex == -1)
                errors.AppendLine("Выберите производителя.");
            if (ComboType.SelectedIndex == -1)
                errors.AppendLine("Выберите тип продукта.");

            if (errors.Length > 0)
            {
                ShowError(errors.ToString());
                return false;
            }
            return true;
        }

        private void SaveGoods()
        {
            if (!ValidateFields()) return;

            try
            {
                int goodsId = _currentGoods.idGoods;
                var goodsToUpdate = AppConnect.modeldb.beautyGoods.FirstOrDefault(u => u.idGoods == goodsId);
                int stock = Convert.ToInt32(instock.Text);
                int cost = Convert.ToInt32(price.Text);

                if (goodsToUpdate != null)
                {
                    goodsToUpdate.article = article.Text;
                    goodsToUpdate.nameGoods = nameTB.Text;
                    goodsToUpdate.price = cost;
                    goodsToUpdate.manufacturer = ComboMan.SelectedIndex + 1;
                    goodsToUpdate.typeGoods = ComboType.SelectedIndex + 1;
                    goodsToUpdate.category = ComboCategory.SelectedIndex + 1;
                    goodsToUpdate.instock = stock;
                    goodsToUpdate.compound = compoundTB.Text;
                    goodsToUpdate.description = desc.Text;
                    goodsToUpdate.image = null;

                    AppConnect.modeldb.SaveChanges();
                    MessageBox.Show("Данные пользователя успешно изменены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppFrame.frameMain.GoBack();
                }
                else
                {
                    ShowError("Пользователь не найден!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void save_Click(object sender, RoutedEventArgs e)
        { 
            SaveGoods();
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

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                nameTB.Focus();
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

        private void ComboCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                instock.Focus();
            }
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
