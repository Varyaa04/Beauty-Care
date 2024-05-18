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
            InitializeComponent(); if (selectedGoods != null)
            {
                _currentGoods = selectedGoods;
            }
            DataContext = _currentGoods;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }
    }
}
