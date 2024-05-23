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
            inputPhone.MaxLength = 11;
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
            if(AppConnect.modeldb.users.Count(x => x.login==inputLogin.Text && x.email==inputEmail.Text) > 0)
            {
                MessageBox.Show("Пользователь с таким логином и email'ом есть!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(inputPhone.Text.Length != 11 || string.IsNullOrWhiteSpace(user.phone))
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
            if (inputEmail.Text.Contains("@") )
            {
                try
                {
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
                catch
                {
                    MessageBox.Show("Ошибка при регистрации!!",
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

        private void inputPswrepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(inputPsw.Text != inputPswrepeat.Password)
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
        }

        private void inputPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        private void inputLogin_TouchEnter(object sender, TouchEventArgs e)
        {

        }
    }
}
