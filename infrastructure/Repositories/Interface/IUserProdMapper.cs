using infrastructure.Entities;

namespace infrastructure.Repositories.Interface
{
    public interface IUserProdMapper
    {
        List<Products> GetUserLikes(int UserId);
        
        List<Products> GetUserCartProducts(int UserId);

        List<UserCartItems> GetUserCartDetails(int userId);

        void AddProductToUserLikes(int userId, int productId);
        void RemoveProductFromLikes(int userId, int productId);
        void AddProductToUserCart(int userId, int productId, int colorId, int sizeId, int quantity);
        void RemoveProductFromCart(int userId, int productId, int colorId, int sizeId, int quantity);
        void UpdateProductQuantity(int userId, int productId, int colorId, int sizeId, int newQuantity);
    }
}