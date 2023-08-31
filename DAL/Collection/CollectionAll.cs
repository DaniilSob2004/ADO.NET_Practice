using Microsoft.Data.SqlClient;
using Store.DAL.Entity;
using System.Collections.Generic;

namespace Store.DAL.Collection
{
    // показ всех товаров
    public class CollectionAll : ICollectionProduct
    {
        private static CollectionAll? collectionAll = null;
        private DAO.ProductDao productDao;
        List<Product> products;

        private CollectionAll(SqlConnection connection)
        {
            products = new List<Product>();
            productDao = new DAO.ProductDao(connection);  // для доступа к работе с БД
        }

        public static ICollectionProduct GetInstance(SqlConnection? connection = null)  // Паттерн Одиночка
        {
            if (collectionAll is null && connection is not null)
            {
                collectionAll = new CollectionAll(connection);
            }
            return collectionAll!;
        }

        public List<Product> GetAll()
        {
            products = productDao.GetAll();  // получение всех товаров
            return products;
        }

        public List<Product> GetAllWithDeleted()
        {
            products = productDao.GetAllWithDeleted();  // получение всех товаров + те которые удалены
            return products;
        }

        public int Count()
        {
            return products.Count;
        }
    }
}
