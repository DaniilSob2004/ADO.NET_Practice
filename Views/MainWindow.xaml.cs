using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Store
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DAL.Entity.Product> Products { get; set; } = new();  // коллекция продуктов
        private DAL.Collection.ICollectionProduct collectionProducts;  // для изменения динамически настройки отображения коллекции

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            DAL.Collection.CollectionDrink.GetInstance(App.GetConnection());  // сразу инициализируем классы для напитков и всех товаров
            collectionProducts = DAL.Collection.CollectionAll.GetInstance(App.GetConnection());
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                e.Cancel = true;
        }

        private void GetAllProducts()
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

        private void GetAllProductsWithDeleted()
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


        private void UpdateCollectionProducts()
        {
            if (checkBoxDeleted.IsChecked ?? false)  // если чекбокс вкл, то значит показываем товары + удалённые
            {
                GetAllProductsWithDeleted();
            }
            else
            {
                GetAllProducts();
            }
            UpdateCountProducts();
        }

        private void UpdateCountProducts()
        {
            textAllCount.Text = collectionProducts.Count().ToString();
        }


        private void BtnAllAmount_Click(object sender, RoutedEventArgs e)
        {
            collectionProducts = DAL.Collection.CollectionAll.GetInstance();  // получаем объект для работы со всей коллекцией
            UpdateCollectionProducts();
        }

        private void BtnAmountDrink_Click(object sender, RoutedEventArgs e)
        {
            collectionProducts = DAL.Collection.CollectionDrink.GetInstance();  // получаем объект для работы с коллекцией напитков
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
