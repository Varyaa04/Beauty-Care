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
        public sign_up()
        {
            InitializeComponent();
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
            }
            catch
            {
                MessageBox.Show("Ошибка при регистрации!!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

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
    }
}
