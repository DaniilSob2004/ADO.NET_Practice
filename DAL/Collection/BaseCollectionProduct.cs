using System.Collections.Generic;

namespace Store.DAL.Collection
{
    public class BaseCollectionProduct
    {
        public virtual List<Entity.Product> GetAll() { return null!; }
        public virtual List<Entity.Product> GetAllWithDeleted() { return null!; }
        public virtual int Count() { return 0; }
    }
}
