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

            phoneT.MaxLength = 12;
            nameT.MaxLength = 25;
            passwT.MaxLength = 30;
            loginT.MaxLength = 30;

            ComboRole.ItemsSource = Entities.GetContext().role.ToList();
            AppConnect.modeldb.users.ToArray();

        }


        private void save_Click(object sender, RoutedEventArgs e)
        {
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
                if (_currentUser.idUser == 0)
                {
                    Entities.GetContext().users.Add(_currentUser);
                }
                try
                {
                    int userId = _currentUser.idUser;
                    var userToUpdate = AppConnect.modeldb.users.FirstOrDefault(u => u.idUser == userId);
                    if (userToUpdate != null)
                    {
                        userToUpdate.login = loginT.Text;
                        userToUpdate.nameUser = nameT.Text;
                        userToUpdate.email = emailT1.Text;
                        userToUpdate.phone = phoneT.Text;
                        userToUpdate.password = passwT.Text;
                        userToUpdate.roleUsers = Convert.ToInt32(ComboRole.SelectedIndex + 1);

                        AppConnect.modeldb.SaveChanges();
                        MessageBox.Show("Данные пользователя успешно изменены!",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        AppFrame.frameMain.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
            }
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
