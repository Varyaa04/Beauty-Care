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
using Beauty_Care.goods;
using Beauty_Care.goodsAdmin;
using Beauty_Care.goodsManager;

namespace Beauty_Care.auth
{
    public partial class sign_in : Page
    {
        public sign_in()
        {
            InitializeComponent();
        }

        private void btnRegistr_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new auth.sign_up());
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var password = AppConnect.modeldb.users.FirstOrDefault(x => x.password == inputPsw.Password);
                if (password == null)
                {
                    MessageBox.Show("неверный пароль",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                var userObj = AppConnect.modeldb.users.FirstOrDefault(x => x.login == inputLogin.Text && x.password == inputPsw.Password);
                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя не существует!", "Ошибка авторизации!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    switch (userObj.roleUsers)
                    {
                        case 1:
                            MessageBox.Show("Здравствуйте, Администратор " + userObj.nameUser + "!",
                                "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            App.Current.Properties["idUser"] = userObj.idUser;
                            App.Current.Properties["roleUser"] = userObj.roleUsers;
                            AppFrame.frameMain.Navigate(new beautyGoodsAdmin((sender as Button).DataContext as users));
                            break;
                        case 2:
                            MessageBox.Show("Здравствуйте, " + userObj.nameUser + "!",
                                "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            App.Current.Properties["idUser"] = userObj.idUser;
                            App.Current.Properties["roleUser"] = userObj.roleUsers;
                            AppFrame.frameMain.Navigate(new beautyGoodsPages((sender as Button).DataContext as users));
                            break;
                        case 3:
                            MessageBox.Show("Здравствуйте, Менеджер " + userObj.nameUser + "!",
                                "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            App.Current.Properties["idUser"] = userObj.idUser;
                            App.Current.Properties["roleUser"] = userObj.roleUsers;
                            AppFrame.frameMain.Navigate(new ordersManager((sender as Button).DataContext as users));
                            break;
                        default:
                            MessageBox.Show("Данные не обнаружены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                            break;
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка " + Ex.Message.ToString() + " Критическая работа приложения!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
