using Beauty_Care.goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            ComboRole.ItemsSource = Entities3.GetContext().role.Select(x => x.nameRole).ToList();

            phoneT.MaxLength = 12;
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

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrWhiteSpace(phoneT.Text) || phoneT.Text.Length < 12)
            {
                MessageBox.Show("Номер телефона должен быть в формате +7XXXXXXXXXX!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(emailT1.Text, emailPattern))
            {
                MessageBox.Show("Введите корректный email!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(loginT.Text) ||
                string.IsNullOrWhiteSpace(nameT.Text) ||
                string.IsNullOrWhiteSpace(passwT.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ComboRole.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите роль!",
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

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                passwT.Focus();
            }
        }

        private void phoneT_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                emailT1.Focus();
            }
        }

        private void BGoBackbutton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void phoneT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!phoneT.Text.StartsWith("+7"))
            {
                phoneT.TextChanged -= phoneT_TextChanged;
                var currentText = phoneT.Text;
                phoneT.Text = "+7" + currentText.TrimStart('+', '7');

                phoneT.TextChanged += phoneT_TextChanged;

                phoneT.SelectionStart = phoneT.Text.Length;
            };


        }

        private void loginT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                nameT.Focus();
            }
        }

        private void passwT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                ComboRole.Focus();
            }
        }

        private void ComboRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                phoneT.Focus();
            }
        }

        private void emailT1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                save.Focus();
            }
        }
    }
}
