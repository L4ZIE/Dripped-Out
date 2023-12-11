using System.Collections.Generic;
using infrastructure.Entities;

namespace infrastructure.Repositories.Interface
{
    public interface IProductMapper
    {
        IEnumerable<Products> getAllProducts();
        List<Products> getProductbyType(int TypeId);

        List<Products> getProductbyGenderType(int TypeId, string Gender);

        List<Products> getProductbyGender(string Gender);
    }
}