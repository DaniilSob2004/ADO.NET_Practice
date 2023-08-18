using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Store
{
    public partial class MainWindow : Window
    {
        private SqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();
            connection = null!;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new SqlConnection(App.ConnectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }

            CreateTables();  // создаём таблицы
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            connection?.Close();
        }


        private void CreateTables()
        {
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
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
                command.ExecuteNonQuery();
                MessageBox.Show("Таблицы созданы!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                InsertDataInTables();  // заполняем таблицы данными
            }
            catch (SqlException)
            {
                MessageBox.Show("Таблицы уже созданы!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InsertDataInTables()
        {
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnAllAmount_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM Product";
            try
            {
                textAllCount.Text = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAmountDrink_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"SELECT COUNT(*)
                                    FROM Product
                                    WHERE id_category = (SELECT TOP 1 id FROM Category WHERE name LIKE 'Напитки')";
            try
            {
                textDrinkCount.Text = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
