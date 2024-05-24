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

namespace Beauty_Care.goodsAdmin
{
    /// <summary>
    /// Логика взаимодействия для editUsers.xaml
    /// </summary>
    public partial class editUsers : Page
    {
        private users _currentUser = new users();
        public editUsers(users selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null)
            {
                _currentUser = selectedUser;
            }
            DataContext = _currentUser;

            phoneT.MaxLength = 11;
            nameT.MaxLength = 25;
            passwT.MaxLength = 30;
            loginT.MaxLength = 30;

            ComboRole.ItemsSource = Entities.GetContext().role.Select(x => x.nameRole).ToList();

        }


        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (phoneT.Text.Length != 11 || string.IsNullOrWhiteSpace(_currentUser.phone))
            {
                MessageBox.Show("Номер телефона состоит из 11 цифр!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(_currentUser.login))
            {
                MessageBox.Show("Введите логин!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(_currentUser.nameUser))
            {
                MessageBox.Show("Введите имя!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(_currentUser.password))
            {
                MessageBox.Show("Введите пароль!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(_currentUser.email))
            {
                MessageBox.Show("Введите почту!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (emailT1.Text.Contains("@"))
            {
                if (_currentUser.idUser == 0)
                {
                    Entities.GetContext().users.Add(_currentUser);
                }
                try
                {
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно изменены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppFrame.frameMain.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Введите почту со специальным символом!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }



        private void name_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.A || e.Key > Key.Z) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
            else
            {
            }
        }

        private void phoneT_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        private void BGoBackbutton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }
    }
}
