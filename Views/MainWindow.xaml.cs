using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Store
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DAL.Entity.Product> Products { get; set; } = new();  // коллекция продуктов

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            CreateTables();  // создаём таблицы
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //
        }


        private void CreateTables()
        {
            using SqlCommand command = new SqlCommand() { Connection = App.Connection };
            command.CommandText = @"CREATE TABLE Category (
                                        id int primary key identity(1, 1),
                                    	name nvarchar(30) NOT NULL
                                    )
                                    CREATE TABLE Product (
                                        id int PRIMARY KEY IDENTITY(1, 1),
                                    	name nvarchar(50) NOT NULL,
                                    	price float NOT NULL,
                                    	quantity int NOT NULL,
                                    	id_category int REFERENCES Category(id)
                                    )";
            try
            {
                command.ExecuteNonQuery();  // выполняем запрос
                MessageBox.Show("Таблицы созданы!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                InsertDataInTables();  // заполняем таблицы данными
            }
            catch (SqlException) { /*MessageBox.Show("Таблицы уже созданы!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);*/ }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void InsertDataInTables()
        {
            using SqlCommand command = new SqlCommand() { Connection = App.Connection };
            command.CommandText = @"INSERT INTO Category (name)
                                    VALUES ('Молочка'), ('Напитки'), ('Крупы')

                                    INSERT INTO Product (name, price, quantity, id_category)
                                    VALUES ('Голандский сыр', 199, 500, 1),
                                           ('Кока-кола', 35.5, 1000, 2),
	                                       ('Молоко', 39.75, 400, 1),
	                                       ('Пепси', 38.49, 1000, 2),
	                                       ('Рис', 85.65, 4000, 3)";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Данные успешно установлены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }


        private void GetAllProducts()
        {
            using SqlCommand command = new SqlCommand() { Connection =  App.Connection };
            command.CommandText = @"SELECT p.name, p.price, p.quantity, c.name AS 'category'
                                    FROM Product AS p
                                    JOIN Category AS c ON c.id = p.id_category
                                    WHERE deleteDt IS NULL";  // запрос на все товары
            try
            {
                UpdateCollectionProducts(command);  // обновляем коллекцию товаров
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void GetDrinkProducts()
        {
            using SqlCommand command = new SqlCommand() { Connection = App.Connection };
            command.CommandText = @"SELECT p.name, p.price, p.quantity, 'Drinkables' AS 'category'
                                    FROM Product AS p
                                    WHERE deleteDt IS NULL AND id_category = (SELECT id
                                                                              FROM Category
                                                                              WHERE name LIKE 'Drinkables')";  // запрос на товары у которых категория - это 'Напитки'
            try
            {
                UpdateCollectionProducts(command);  // обновляем коллекцию товаров
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void UpdateCollectionProducts(SqlCommand command)
        {
            Products.Clear();

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                // заполняем данными из БД коллекцию продуктов
                Products.Add(new()
                {
                    Name = reader.GetString("Name"),
                    Price = (float)reader.GetDouble("Price"),
                    Quantity = reader.GetInt32("Quantity"),
                    Category = reader.GetString("Category")
                });
            }
        }


        private void BtnAllAmount_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new SqlCommand() { Connection = App.Connection };
            command.CommandText = "SELECT COUNT(*) FROM Product WHERE deleteDt IS NULL";  // кол-во всех товаров
            try
            {
                textAllCount.Text = command.ExecuteScalar().ToString();  // обновляем значение в интерфейсе
                GetAllProducts();  // обновляем список товаров
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BtnAmountDrink_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new SqlCommand() { Connection = App.Connection };
            command.CommandText = @"SELECT COUNT(*)
                                    FROM Product
                                    WHERE deleteDt IS NULL AND id_category = (SELECT id
					                                                          FROM Category
					                                                          WHERE name LIKE 'Drinkables')";  // кол-во товаров, категория которых - это 'Напитки'
            try
            {
                textDrinkCount.Text = command.ExecuteScalar().ToString();  // обновляем значение в интерфейсе
                GetDrinkProducts();  // обновляем список товаров
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Close();
        }
    }
}
