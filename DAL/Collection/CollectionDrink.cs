using Microsoft.Data.SqlClient;
using Store.DAL.Entity;
using System.Collections.Generic;

namespace Store.DAL.Collection
{
    // показ напитков
    public class CollectionDrink : ICollectionProduct
    {
        private static CollectionDrink? collectionDrink = null;
        private DAO.ProductDao productDao;
        List<Product> products;

        private CollectionDrink(SqlConnection connection)
        {
            products = new List<Product>();
            productDao = new DAO.ProductDao(connection);  // для доступа к работе с БД
        }

        public static ICollectionProduct GetInstance(SqlConnection? connection = null)  // Паттерн Одиночка
        {
            if (collectionDrink is null && connection is not null)
            {
                collectionDrink = new CollectionDrink(connection);
            }
            return collectionDrink!;
        }

        public List<Product> GetAll()
        {
            products = productDao.GetAllDrinkables();  // получение всех товаров напитков
            return products;
        }

        public List<Product> GetAllWithDeleted()
        {
            products = productDao.GetAllDrinkablesWithDeleted();  // получение всех товаров напитков + те которые удалены
            return products;
        }

        public int Count()
        {
            return products.Count;
        }
    }
}
