using infrastructure.Entities;
using infrastructure.Entities.Helper;
using infrastructure.Repositories;
using infrastructure.Repositories.Factory;
using infrastructure.Repositories.Interface;

namespace service.Services;

public class UserProdService
{
    private readonly IUserProdMapper _UserCartRepository;


    public UserProdService(UserProdRepository userProdRepository)
    {
        _UserCartRepository = userProdRepository;
    }


    public List<Products> GetUserLikes(int userId)
    {
        return _UserCartRepository.GetUserLikes(userId);
    }

    public List<Products> GetUserCartProducts(int userId)
    {
        return _UserCartRepository.GetUserCartProducts(userId);
    }

    public List<UserCartItems> GetUserCartDetails(int userId)
    {
        return _UserCartRepository.GetUserCartDetails(userId);
    }

    public void AddProductToUserCart(int userId, int productId, int colorId, int sizeId, int quantity)
    {
        _UserCartRepository.AddProductToUserCart(userId, productId, colorId, sizeId, quantity);
    }

    public void RemoveProductFromCart(int userId, int productId, int colorId, int sizeId, int quantity)
    {
        _UserCartRepository.RemoveProductFromCart(userId, productId, colorId, sizeId, quantity);
    }

    public void UpdateProductQuantity(int userId, int productId, int colorId, int sizeId, int quantity)
    {
        _UserCartRepository.UpdateProductQuantity(userId, productId, colorId, sizeId, quantity);
    }

    public void AddProductToUserLikes(int userId, int productId)
    {
        _UserCartRepository.AddProductToUserLikes(userId, productId);
    }

    public void RemoveProductFromLikes(int userId, int productId)
    {
        _UserCartRepository.RemoveProductFromLikes(userId, productId);
    }



}