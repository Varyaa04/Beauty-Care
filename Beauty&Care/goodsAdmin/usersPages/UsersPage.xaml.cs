using Beauty_Care.cartPage;
using Beauty_Care.goods;
using Beauty_Care.goodsAdmin.usersPages;
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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            ListUsers.ItemsSource = AppConnect.modeldb.users.ToList();
        }

        private void pageVisible(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ListUsers.ItemsSource = Entities.GetContext().users.ToList();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new editUsers((sender as Button).DataContext as users));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new addUser());
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Вы точно хотите удалить выбранного пользователя?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ListUsers.ItemsSource = Entities.GetContext().users.ToList();
                Button b = sender as Button;
                int ID = int.Parse(((b.Parent as StackPanel).Children[0] as TextBlock).Text);
                Console.WriteLine(ID);
                AppConnect.modeldb.users.Remove(
                    AppConnect.modeldb.users.Where(x => x.idUser == ID).First());
                AppConnect.modeldb.SaveChanges();
                AppFrame.frameMain.GoBack();
                AppFrame.frameMain.Navigate(new UsersPage());
            }

            var userDel = ListUsers.SelectedItems.Cast<users>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить выбранных пользoвателей ({userDel.Count()})?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var context = Entities.GetContext();
                    foreach (var item in userDel)
                    {
                        var user = context.users.Find(item.idUser);
                        if (user != null)
                        {
                            context.users.Remove(user);
                        }
                    }
                    context.SaveChanges();
                    MessageBox.Show("Данные удалены");
                    ListUsers.SelectedItem = null;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
