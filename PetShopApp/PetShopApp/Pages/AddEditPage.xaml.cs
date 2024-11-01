using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private bool IsAdding { get; set; }
        private Model.Product CurrentProduct { get; set; }
        private Model.User CurrentUser { get; set; }
        public AddEditPage(Model.Product product, bool isadding, Model.User user)
        {
            InitializeComponent();
            if (isadding)
            {
                IsAdding = true;
                CurrentProduct = new Model.Product();
            }
            else
            {
                IsAdding = false;
                CurrentProduct = product;
            }
            DataContext = CurrentProduct;
            CurrentUser = user;
            Init();
        }
        private void Init()
        {
            try
            {
                var Categories = Model.TradeEntities.GetContext().Categories.ToList();
                Categories.Insert(0, new Model.Categories() { Name = "Выберите категорию" });
                CategoryCB.ItemsSource = Categories;
                if (IsAdding == false)
                {
                    IdTB.Text = CurrentProduct.ProductID.ToString();
                    NameTB.Text = CurrentProduct.ProductNames.Name;
                    CategoryCB.SelectedIndex = CurrentProduct.ProductCategoryID;
                    CountityTB.Text = CurrentProduct.ProductQuantityInStock.ToString();
                    UnitTB.Text = CurrentProduct.Units.Name;
                    ImporterTB.Text = CurrentProduct.Importers.Name;
                    CostTB.Text = CurrentProduct.ProductCost.ToString();
                    DescriptionTB.Text = CurrentProduct.ProductDescription;
                }
                else
                {
                    IdTB.Text = (Model.TradeEntities.GetContext().Product.ToList().Last().ProductID + 1).ToString();
                    NameTB.Text = string.Empty;
                    CategoryCB.SelectedIndex = 0;
                    CountityTB.Text = string.Empty;
                    UnitTB.Text = string.Empty;
                    ImporterTB.Text = string.Empty;
                    CostTB.Text = string.Empty;
                    DescriptionTB.Text = string.Empty;
                }
            }
            catch (Exception)
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                if (CategoryCB.SelectedIndex == 0) { errors.AppendLine("Выберите категорию"); }
                if (string.IsNullOrEmpty(NameTB.Text)) { errors.AppendLine("Заполните наименование"); }
                if (string.IsNullOrEmpty(CountityTB.Text)) { errors.AppendLine("Заполните кол-во"); }
                else
                {
                    var count = int.TryParse(CountityTB.Text, out int result);
                    if (!count)
                    {
                        errors.AppendLine("Количество не число!");
                    }
                    else
                    {
                        if (result < 0)
                        {
                            errors.AppendLine("Количество не может быть отрицательным!");
                        }
                    }
                }
                if (string.IsNullOrEmpty(UnitTB.Text)) { errors.AppendLine("Заполните ед. измерения"); }
                if (string.IsNullOrEmpty(ImporterTB.Text)) { errors.AppendLine("Заполните поставщика"); }
                if (string.IsNullOrEmpty(CostTB.Text)) { errors.AppendLine("Заполните стоимость"); }
                else
                {
                    var Cost = decimal.TryParse(CostTB.Text, out decimal result);
                    if (!Cost)
                    {
                        errors.AppendLine("Стоимость не число!");
                    }
                    else
                    {
                        var splittedCost = CostTB.Text.Split(',');
                        if (splittedCost.Length>1)
                        {
                            if (splittedCost[1].Length > 2)
                            {
                                errors.AppendLine("Количество знаков после запятой в стоимости привышает сотые части!");
                            }
                        }
                        if (result < 0)
                        {
                            errors.AppendLine("Цена не может быть отрицательной!");
                        }
                    }
                }
                if (string.IsNullOrEmpty(DescriptionTB.Text)) { errors.AppendLine("Заполните описание"); }
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CurrentProduct.ProductCategoryID = CategoryCB.SelectedIndex;
                var IsNameAllready = Model.TradeEntities.GetContext().ProductNames.ToList().Where(i => i.Name == NameTB.Text).FirstOrDefault();
                if (IsNameAllready == null)
                {
                    Model.ProductNames newProductName = new Model.ProductNames() { Name = NameTB.Text };
                    Model.TradeEntities.GetContext().ProductNames.Add(newProductName);
                    Model.TradeEntities.GetContext().SaveChanges();
                    CurrentProduct.ProductNameID = newProductName.Id;
                }
                else
                {
                    CurrentProduct.ProductNameID = IsNameAllready.Id;
                }
                CurrentProduct.ProductQuantityInStock = Convert.ToInt32(CountityTB.Text);
                var IsUnitAllready = Model.TradeEntities.GetContext().Units.ToList().Where(i => i.Name == UnitTB.Text).FirstOrDefault();
                if (IsUnitAllready == null)
                {
                    Model.Units newUnitName = new Model.Units() { Name = UnitTB.Text };
                    Model.TradeEntities.GetContext().Units.Add(newUnitName);
                    Model.TradeEntities.GetContext().SaveChanges();
                    CurrentProduct.ProductUnitID = newUnitName.Id;
                }
                else
                {
                    CurrentProduct.ProductUnitID = IsUnitAllready.Id;
                }
                var IsImporterAllready = Model.TradeEntities.GetContext().Importers.ToList().Where(i => i.Name == ImporterTB.Text).FirstOrDefault();
                if (IsImporterAllready == null)
                {
                    Model.Importers newImporter = new Model.Importers() { Name = ImporterTB.Text };
                    Model.TradeEntities.GetContext().Importers.Add(newImporter);
                    Model.TradeEntities.GetContext().SaveChanges();
                    CurrentProduct.ProductImporterID = newImporter.Id;
                }
                else
                {
                    CurrentProduct.ProductImporterID = IsImporterAllready.Id;
                }
                CurrentProduct.ProductCost = Convert.ToDecimal(CostTB.Text);
                CurrentProduct.ProductDescription = DescriptionTB.Text;
                if (IsAdding)
                {
                    CurrentProduct.ProductID = Model.TradeEntities.GetContext().Product.ToList().Last().ProductID + 1;
                    Model.TradeEntities.GetContext().Product.Add(CurrentProduct);
                }
                var products = Model.TradeEntities.GetContext().Product.ToList();
                Model.TradeEntities.GetContext().SaveChanges();
                Utils.Navigation.CurrentFrame.RemoveBackEntry();
                Utils.Navigation.CurrentFrame.Navigate(new Pages.ProductAdminPage(CurrentUser));
                MessageBox.Show("Продукт сохранен!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());
            }

        }
        private (string path, byte[] image) FileImageToVars()
        {
            return ("s", new byte[1]);
        }

        private void PhotoImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Изображение (@.jpeg; @.png) | .jpeg; .png";
        }
    }
}
