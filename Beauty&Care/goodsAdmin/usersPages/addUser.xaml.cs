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

namespace Beauty_Care.goodsAdmin.usersPages
{
    /// <summary>
    /// Логика взаимодействия для addUser.xaml
    /// </summary>
    public partial class addUser : Page
    {
        users user = new users();

        public addUser()
        {
            InitializeComponent();
            ComboRole.ItemsSource = Entities.GetContext().role.Select(x => x.nameRole).ToList();

            phoneT.MaxLength = 11;
            nameT.MaxLength = 25;
            passwT.MaxLength = 30;
            loginT.MaxLength = 30;
        }

        private void ComboMan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.modeldb.users.Count(x => x.login == loginT.Text && x.email == emailT1.Text) > 0)
            {
                MessageBox.Show("Пользователь с таким логином и email'ом есть!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (phoneT.Text.Length != 11 || string.IsNullOrWhiteSpace(user.phone))
            {
                MessageBox.Show("Номер телефона состоит из 11 цифр!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(user.login))
            {
                MessageBox.Show("Введите логин!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(user.nameUser))
            {
                MessageBox.Show("Введите имя!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(user.password))
            {
                MessageBox.Show("Введите пароль!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(user.email))
            {
                MessageBox.Show("Введите почту!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (emailT1.Text.Contains("@"))
            {
                try
                {
                    users userObj = new users()
                    {
                        login = loginT.Text,
                        nameUser = nameT.Text,
                        email = emailT1.Text,
                        phone = phoneT.Text,
                        password = passwT.Text,
                        roleUsers = Convert.ToInt32(ComboRole.SelectedIndex + 1),
                    };
                    AppConnect.modeldb.users.Add(userObj);
                    AppConnect.modeldb.SaveChanges();
                    MessageBox.Show("Пользователь добавлен!",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppFrame.frameMain.GoBack();
                }
                catch
                {
                    MessageBox.Show("Ошибка при добавлении!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

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
