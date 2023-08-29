using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Store.DAL.DAO
{
    internal class ProductDao
    {
        private readonly SqlConnection _connection;

        public ProductDao(SqlConnection connection)
        {
            _connection = connection;
        }

        public void CreateTable()
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"CREATE TABLE Category (
                                        id int PRIMARY KEY IDENTITY(1, 1),
                                    	name nvarchar(30) NOT NULL
                                    )
                                    CREATE TABLE Product (
                                        id int PRIMARY KEY IDENTITY(1, 1),
                                    	name nvarchar(50) NOT NULL,
                                    	price float NOT NULL,
                                    	quantity int NOT NULL,
                                    	id_category int REFERENCES Category(id),
                                        deleteDt DateTime
                                    )";
            command.ExecuteNonQuery();
        }

        public List<Entity.Product> GetAllWithDeleted()
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"SELECT p.name, p.price, p.quantity, c.name AS 'category'
                                    FROM Product AS p
                                    JOIN Category AS c ON c.id = p.id_category";  // запрос на все товары
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                var products = new List<Entity.Product>();
                while (reader.Read())
                {
                    products.Add(new()
                    {
                        Name = reader.GetString("Name"),
                        Price = (float)reader.GetDouble("Price"),
                        Quantity = reader.GetInt32("Quantity"),
                        Category = reader.GetString("Category")
                    });
                }
                return products;
            }
            catch { throw; }
        }

        public List<Entity.Product> GetAll()
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"SELECT p.name, p.price, p.quantity, c.name AS 'category'
                                    FROM Product AS p
                                    JOIN Category AS c ON c.id = p.id_category
                                    WHERE deleteDt IS NULL";  // запрос на все неудалённые товары
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                var products = new List<Entity.Product>();
                while (reader.Read())
                {
                    // заполняем данными из БД коллекцию продуктов
                    products.Add(new()
                    {
                        Name = reader.GetString("Name"),
                        Price = (float)reader.GetDouble("Price"),
                        Quantity = reader.GetInt32("Quantity"),
                        Category = reader.GetString("Category")
                    });
                }
                return products;
            }
            catch { throw; }
        }

        public List<Entity.Product> GetAllDrinkables()
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"SELECT p.name, p.price, p.quantity, 'Drinkables' AS 'category'
                                    FROM Product AS p
                                    WHERE deleteDt IS NULL AND id_category = (SELECT id
                                                                              FROM Category
                                                                              WHERE name LIKE 'Drinkables')";  // запрос на товары у которых категория - это 'Напитки'
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                var products = new List<Entity.Product>();
                while (reader.Read())
                {
                    // заполняем данными из БД коллекцию продуктов
                    products.Add(new()
                    {
                        Name = reader.GetString("Name"),
                        Price = (float)reader.GetDouble("Price"),
                        Quantity = reader.GetInt32("Quantity"),
                        Category = reader.GetString("Category")
                    });
                }
                return products;
            }
            catch { throw; }
        }

        public List<Entity.Product> GetAllDrinkablesWithDeleted()
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"SELECT p.name, p.price, p.quantity, 'Drinkables' AS 'category'
                                    FROM Product AS p
                                    WHERE id_category = (SELECT id
                                                         FROM Category
                                                         WHERE name LIKE 'Drinkables')";  // запрос на товары у которых категория - это 'Напитки'
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                var products = new List<Entity.Product>();
                while (reader.Read())
                {
                    // заполняем данными из БД коллекцию продуктов
                    products.Add(new()
                    {
                        Name = reader.GetString("Name"),
                        Price = (float)reader.GetDouble("Price"),
                        Quantity = reader.GetInt32("Quantity"),
                        Category = reader.GetString("Category")
                    });
                }
                return products;
            }
            catch { throw; }
        }

        public void Add(Entity.Product product, int id_cat)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = $@"INSERT INTO Product (name, price, quantity, id_category, deleteDt)
                                     VALUES (@name, @price, @quantity, {id_cat}, NULL)";
            command.Prepare();

            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@price", SqlDbType.Float));
            command.Parameters.Add(new SqlParameter("@quantity", SqlDbType.Int));

            command.Parameters[0].Value = product.Name;
            command.Parameters[1].Value = product.Price;
            command.Parameters[2].Value = product.Quantity;

            command.ExecuteNonQuery();
        }
    }
}
