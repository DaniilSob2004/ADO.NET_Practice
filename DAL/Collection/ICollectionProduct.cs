using System.Collections.Generic;

namespace Store.DAL.Collection
{
    // интерфейс для изменения настройки отображения коллекции
    public interface ICollectionProduct
    {
        public List<Entity.Product> GetAll();
        public List<Entity.Product> GetAllWithDeleted();
        public int Count();
    }
}
