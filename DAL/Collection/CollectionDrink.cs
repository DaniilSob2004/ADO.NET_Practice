using Microsoft.Data.SqlClient;
using Store.DAL.Entity;
using System.Collections.Generic;

namespace Store.DAL.Collection
{
    public class CollectionDrink : BaseCollectionProduct
    {
        private static CollectionDrink? collectionDrink = null;
        private DAO.ProductDao productDao;
        List<Product> products = new List<Product>();

        private CollectionDrink(SqlConnection connection)
        {
            productDao = new DAO.ProductDao(connection);
        }

        public static BaseCollectionProduct GetInstance(SqlConnection? connection = null)
        {
            if (collectionDrink is null && connection is not null)
            {
                collectionDrink = new CollectionDrink(connection);
            }
            return collectionDrink!;
        }

        public override List<Product> GetAll()
        {
            products = productDao.GetAllDrinkables();
            return products;
        }

        public override List<Product> GetAllWithDeleted()
        {
            products = productDao.GetAllDrinkablesWithDeleted();
            return products;
        }

        public override int Count()
        {
            return products.Count;
        }
    }
}
