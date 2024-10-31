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
            var Manufacturies = Model.TradeEntities.GetContext().Manufacturies.ToList();
            Manufacturies.Insert(0, new Model.Manufacturies() {Name = "Все производители" });
            ManufacturiesFilter.ItemsSource = Manufacturies;
            ManufacturiesFilter.SelectedIndex = 0;
        }

        private void Update()
        {
            try
            {
                ProductsWithFilter = Model.TradeEntities.GetContext().Product
                    .Where(i => i.ProductDescription.ToString().ToLower().Contains(SearchTB.Text.ToLower()) ||
          i.Manufacturies.Name.ToString().ToLower().Contains(SearchTB.Text.ToLower()) ||
          i.ProductCost.ToString().Contains(SearchTB.Text) ||
          i.ProductNames.Name.ToString().ToLower().Contains(SearchTB.Text.ToLower()) ||
          i.ProductQuantityInStock.ToString().Contains(SearchTB.Text))
              .ToList();

                if (ByAsc.IsChecked == true)
                {
                    ProductsWithFilter = ProductsWithFilter.OrderBy(i => i.ProductCost).ToList();
                }
                else if (ByDesc.IsChecked == true)
                {
                    ProductsWithFilter = ProductsWithFilter.OrderByDescending(i => i.ProductCost).ToList();
                }
                if(ManufacturiesFilter.SelectedIndex != 0)
                {
                    ProductsWithFilter = ProductsWithFilter.ToList().Where(i => i.ProductManufacturerID == ManufacturiesFilter.SelectedIndex).ToList();
                }
                AllProductCountity.Text = ProductsWithFilter.Count.ToString();
                ProductsListView.ItemsSource = ProductsWithFilter;
            }
            catch (Exception ex)
            {
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
