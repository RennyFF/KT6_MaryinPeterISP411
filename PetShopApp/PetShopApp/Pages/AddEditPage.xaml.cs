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
        private bool IsAdding { get; set; } = true;
        private Model.Product CurrentProduct { get; set; }
        public AddEditPage(Model.Product product)
        {
            InitializeComponent();
            if (product != null)
            {
                IsAdding = false;
                DataContext = product;
                CurrentProduct = product;
            }
            else
            {
                CurrentProduct = new Model.Product();
            }
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
                    //PhotoImage = product.ProductPhoto;
                    DescriptionTB.Text = CurrentProduct.ProductDescription;
                }
                else
                {
                    IdTB.Text = (Model.TradeEntities.GetContext().Product.Last().ProductID + 1).ToString();
                }
            }
            catch (Exception)
            {
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                int CategoryIndex = CategoryCB.SelectedIndex;
                int Id = Convert.ToInt32(IdTB.Text);
                string Name = NameTB.Text;
                string Countity = CountityTB.Text;
                string Unit = UnitTB.Text;
                string Importer = ImporterTB.Text;
                string Cost = CostTB.Text;
                string Description = DescriptionTB.Text;
                if (CategoryIndex == 0) { errors.AppendLine("Выберите категорию"); }
                if (string.IsNullOrEmpty(Name)) { errors.AppendLine("Заполните наименование"); }
                if (string.IsNullOrEmpty(Countity)) { errors.AppendLine("Заполните кол-во"); }
                else
                {
                    if (!int.TryParse(Countity, out int result))
                    {
                        errors.AppendLine("Количество не число!");
                    }
                    else
                    {
                        if (Countity.Contains('-'))
                        {
                            errors.AppendLine("Количество не может быть отрицательным!");
                        }
                    }
                }
                if (string.IsNullOrEmpty(Unit)) { errors.AppendLine("Заполните ед. измерения"); }
                if (string.IsNullOrEmpty(Importer)) { errors.AppendLine("Заполните поставщика"); }
                if (string.IsNullOrEmpty(Cost)) { errors.AppendLine("Заполните стоимость"); }
                else
                {
                    if (!decimal.TryParse(Cost, out decimal result))
                    {
                        errors.AppendLine("Стоимость не число!");
                    }
                    else
                    {
                        var splittedCost = Cost.Split(',');
                        if (splittedCost[1] != null)
                        {
                            if (splittedCost[1].Length > 2)
                            {
                                errors.AppendLine("Количество знаков после запятой в стоимости привышает сотые части!");
                            }
                        }
                        if (Cost.Contains('-'))
                        {
                            errors.AppendLine("Цена не может быть отрицательной!");
                        }
                    }
                }
                if (string.IsNullOrEmpty(Description)) { errors.AppendLine("Заполните описание"); }
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (IsAdding)
                {
                    CurrentProduct.ProductID = Id;
                    bool IsNameAllready = Model.TradeEntities.GetContext().ProductNames.ToList().Where(i => i.Name == Name).FirstOrDefault() != null;
                    if (!IsNameAllready)
                    {
                        Model.TradeEntities.GetContext().ProductNames.Add(new Model.ProductNames() { Name = this.Name });
                        Model.TradeEntities.GetContext().SaveChanges();
                    }
                    CurrentProduct.ProductNameID = Model.TradeEntities.GetContext().ProductNames.ToList().Where(i => i.Name == Name).First().Id;
                    CurrentProduct.ProductQuantityInStock = Convert.ToInt32(Countity);
                    bool IsUnitAllready = Model.TradeEntities.GetContext().Units.ToList().Where(i => i.Name == Unit).FirstOrDefault() != null;
                    if (!IsUnitAllready)
                    {
                        Model.TradeEntities.GetContext().Units.Add(new Model.Units() { Name = Unit });
                        Model.TradeEntities.GetContext().SaveChanges();
                    }
                    CurrentProduct.ProductUnitID = Model.TradeEntities.GetContext().Units.ToList().Where(i => i.Name == Unit).First().Id;
                    bool IsImporterAllready = Model.TradeEntities.GetContext().Importers.ToList().Where(i => i.Name == Importer).FirstOrDefault() != null;
                    if (!IsImporterAllready)
                    {
                        Model.TradeEntities.GetContext().Importers.Add(new Model.Importers() { Name = Importer });
                        Model.TradeEntities.GetContext().SaveChanges();
                    }
                    CurrentProduct.ProductImporterID = Model.TradeEntities.GetContext().Importers.ToList().Where(i => i.Name == Importer).First().Id;
                    CurrentProduct.ProductCost = Convert.ToInt32(Cost);
                    CurrentProduct.ProductDescription = Description;
                    var ImageResult = FileImageToVars();
                    CurrentProduct.ProductPhotoName = ImageResult.path;
                    CurrentProduct.ProductPhoto = ImageResult.image;

                }
                else
                {

                }
            }
            catch (Exception)
            {
            }

        }
        private (string path, byte[] image) FileImageToVars()
        {
            return ("s", new byte[1]);
        }
    }
}
