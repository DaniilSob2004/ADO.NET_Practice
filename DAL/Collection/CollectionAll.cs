using Microsoft.Data.SqlClient;
using Store.DAL.Entity;
using System.Collections.Generic;

namespace Store.DAL.Collection
{
    public class CollectionAll : BaseCollectionProduct
    {
        private static CollectionAll? collectionAll = null;
        private DAO.ProductDao productDao;
        List<Product> products = new List<Product>();

        private CollectionAll(SqlConnection connection)
        {
            productDao = new DAO.ProductDao(connection);
        }

        public static BaseCollectionProduct GetInstance(SqlConnection? connection = null)
        {
            if (collectionAll is null && connection is not null)
            {
                collectionAll = new CollectionAll(connection);
            }
            return collectionAll!;
        }

        public override List<Product> GetAll()
        {
            products = productDao.GetAll();
            return products;
        }

        public override List<Product> GetAllWithDeleted()
        {
            products = productDao.GetAllWithDeleted();
            return products;
        }

        public override int Count()
        {
            return products.Count;
        }
    }
}
