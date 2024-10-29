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
    /// Логика взаимодействия для ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        private List<Model.Product> ProductsWithFilter;
        private Model.User CurrentUser;
        public ProductListPage(Model.User user)
        {
            InitializeComponent();
            CurrentUser = user;
            Init();
        }

        private void Init()
        {
            ProductsWithFilter = Model.TradeEntities.GetContext().Product.ToList();
            ProductsListView.ItemsSource = ProductsWithFilter;
            FIOLabel.Text = CurrentUser.UserSurname + " " + CurrentUser.UserName + " " + CurrentUser.UserPatronymic;
            AllProductCountity.Text = ProductsWithFilter.Count.ToString();
        }

        private void Update()
        {
            ProductsWithFilter = Model.TradeEntities.GetContext().Product.Where(i => i.ProductDescription.Contains(SearchTB.Text) || 
            i.Manufacturies.Name.Contains(SearchTB.Text) || 
            i.ProductCost.ToString().Contains(SearchTB.Text)  ||
            i.ProductNames.Name.Contains(SearchTB.Text) ||
            i.ProductQuantityInStock.ToString().Contains(SearchTB.Text))
                .ToList();

            //not working
            if ((bool)ByAsc.IsChecked)
            {
                ProductsWithFilter.OrderBy(i => i).ToList();
            }
            else if ((bool)ByDesc.IsChecked) {
                ProductsWithFilter.OrderByDescending(i => i).ToList();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.Navigation.CurrentFrame.CanGoBack)
            {
                Utils.Navigation.CurrentFrame.GoBack();
            }
        }

        private void ManufacturiesFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ByDesc_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void ByAsc_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
