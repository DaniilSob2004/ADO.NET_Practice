using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace Store
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DAL.Entity.Product> Products { get; set; } = new();  // коллекция продуктов
        private DAL.DAO.ProductDao productDao;
        private DAL.Collection.BaseCollectionProduct collectionProducts;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            productDao = new(App.GetConnection());
            DAL.Collection.CollectionDrink.GetInstance(App.GetConnection());
            collectionProducts = DAL.Collection.CollectionAll.GetInstance(App.GetConnection());

            CreateTables();  // создаём таблицы
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                e.Cancel = true;
        }


        private void CreateTables()
        {
            try
            {
                productDao.CreateTable();  // создаём таблицы продуктов и категории
                MessageBox.Show("Таблицы созданы!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                InsertDataInTables();  // заполняем таблицы данными
            }
            catch (SqlException) { /*MessageBox.Show("Таблицы уже созданы!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);*/ }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void InsertDataInTables()
        {
            try
            {
                // записываем пару записей в БД
                DAL.Entity.Product newProduct = new() { Name = "Голандский сыр", Price = 199, Quantity = 500 };
                productDao.Add(newProduct, 1);
                newProduct = new() { Name = "Кока-кола", Price = 35.5f, Quantity = 1000 };
                productDao.Add(newProduct, 2);

                MessageBox.Show("Данные успешно установлены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void GetAllProducts()
        {
            try
            {
                Products.Clear();
                foreach (var product in collectionProducts.GetAllWithDeleted())
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void GetAllProductsWithDeleted()
        {
            try
            {
                Products.Clear();
                foreach (var product in collectionProducts.GetAll())
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }


        private void UpdateCollectionProducts()
        {
            if (checkBoxDeleted.IsChecked ?? false)
            {
                GetAllProducts();
            }
            else
            {
                GetAllProductsWithDeleted();
            }
            UpdateCountProducts();
        }

        private void UpdateCountProducts()
        {
            textAllCount.Text = collectionProducts.Count().ToString();
        }


        private void BtnAllAmount_Click(object sender, RoutedEventArgs e)
        {
            collectionProducts = DAL.Collection.CollectionAll.GetInstance();
            UpdateCollectionProducts();
        }

        private void BtnAmountDrink_Click(object sender, RoutedEventArgs e)
        {
            collectionProducts = DAL.Collection.CollectionDrink.GetInstance();
            UpdateCollectionProducts();
        }

        private void BtnExit_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                UpdateCollectionProducts();
            }
        }
    }
}
