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
using Beauty_Care.goods;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Beauty_Care.auth
{
    /// <summary>
    /// Логика взаимодействия для sign_up.xaml
    /// </summary>
    public partial class sign_up : Page
    {
        users user = new users();
        public sign_up()
        {
            InitializeComponent();
            inputPhone.MaxLength = 12;
            inputName.MaxLength = 25;
            inputPsw.MaxLength = 30;
            inputLogin.MaxLength = 30;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppConnect.modeldb.users.Any(x => x.login == inputLogin.Text && x.email == inputEmail.Text))
                {
                    MessageBox.Show("Пользователь с таким логином и email'ом уже существует!",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(inputLogin.Text) ||
                    string.IsNullOrWhiteSpace(inputName.Text) ||
                    string.IsNullOrWhiteSpace(inputPsw.Text))
                {
                    MessageBox.Show("Все поля должны быть заполнены!",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if(inputPsw.Text != inputPswrepeat.Password)
                {
                    MessageBox.Show("Пароли не совпадают!",
                       "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                if (!Regex.IsMatch(inputEmail.Text, emailPattern))
                {
                    MessageBox.Show("Введите корректный email!",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(inputPhone.Text) || inputPhone.Text.Length < 12)
                {
                    MessageBox.Show("Номер телефона должен быть в формате +7XXXXXXXXXX!",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }




                users userObj = new users()
                {
                    login = inputLogin.Text,
                    nameUser = inputName.Text,
                    email = inputEmail.Text,
                    phone = inputPhone.Text,
                    password = inputPsw.Text,
                    roleUsers = 2
                };
                AppConnect.modeldb.users.Add(userObj);
                AppConnect.modeldb.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frameMain.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации: " + ex.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void inputPswrepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (inputPsw.Text != inputPswrepeat.Password || inputPswrepeat.Password == "")
            {
                btnRegister.IsEnabled = false;
                inputPswrepeat.Background = Brushes.LightCoral;
                inputPswrepeat.BorderBrush = Brushes.Red;
            }
            else
            {
                btnRegister.IsEnabled = true;
                inputPswrepeat.Background = Brushes.LightGreen;
                inputPswrepeat.BorderBrush = Brushes.Green;
            }
        }

        private void inputName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.A || e.Key > Key.Z) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
            else
            {
            }

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                inputEmail.Focus();
            }
        }

        private void inputPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                inputPsw.Focus();
            }
        }


        private void inputPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!inputPhone.Text.StartsWith("+7"))
            {
                inputPhone.TextChanged -= inputPhone_TextChanged;
                var currentText = inputPhone.Text;
                inputPhone.Text = "+7" + currentText.TrimStart('+', '7');

                inputPhone.TextChanged += inputPhone_TextChanged;

                inputPhone.SelectionStart = inputPhone.Text.Length;
            }
        }

        private void inputLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                inputName.Focus();
            }
        }

        private void inputEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                inputPhone.Focus();
            }
        }

        private void inputPsw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                inputPswrepeat.Focus();
            }
        }

        private void inputPswrepeat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                btnRegister.Focus();
            }
        }

        private void inputPsw_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inputPsw.Text != inputPswrepeat.Password || inputPswrepeat.Password == "")
            {
                btnRegister.IsEnabled = false;
                inputPswrepeat.Background = Brushes.LightCoral;
                inputPswrepeat.BorderBrush = Brushes.Red;
            }
            else
            {
                btnRegister.IsEnabled = true;
                inputPswrepeat.Background = Brushes.LightGreen;
                inputPswrepeat.BorderBrush = Brushes.Green;
            }
        }
    }
}
