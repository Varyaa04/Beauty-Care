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

namespace Beauty_Care.goodsManager
{
    /// <summary>
    /// Логика взаимодействия для ordersManager.xaml
    /// </summary>
    public partial class ordersManager : Page
    {
        private users _authManager = new users();

        public ordersManager(users authUser)
        {
            InitializeComponent();


            if (authUser != null)
            {
                _authManager = authUser;
            }
            DataContext = _authManager;
        }
    }
}
