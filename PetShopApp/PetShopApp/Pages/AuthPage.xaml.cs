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

namespace PetShopApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void GuestAuth_Click(object sender, RoutedEventArgs e)
        {
            Utils.Navigation.CurrentFrame.Navigate(new Pages.ProductListPage(new Model.User() { UserSurname="Вы", UserName = "в роли", UserPatronymic="гостя" }));
        }

        private void AuthUser_Click(object sender, RoutedEventArgs e)
        {
            string Pass = PasswordPB.Password;
            string Login = LoginTB.Text;
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(Pass))
            {
                errors.AppendLine("Заполните пароль!");
            }
            if (string.IsNullOrEmpty(Login)) {
                errors.AppendLine("Заполните логин!");
            }
            if (errors.Length > 0) {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var User = Model.TradeEntities.GetContext().User.ToList().Where(i => i.UserPassword == Pass && i.UserLogin == Login).FirstOrDefault();
            if (User != null) {
                MessageBox.Show("Успешный вход!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (User.Role.RoleName) {
                    case "Администратор":
                        Utils.Navigation.CurrentFrame.Navigate(new Pages.ProductAdminPage(User));
                        break;
                    case "Клиент":
                        Utils.Navigation.CurrentFrame.Navigate(new Pages.ProductListPage(User));
                        break;
                    case "Менеджер":
                        Utils.Navigation.CurrentFrame.Navigate(new Pages.ProductListPage(User));
                        break;
                    default: break;
                }
                PasswordPB.Password = string.Empty;
                LoginTB.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Пользователь не найден!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
