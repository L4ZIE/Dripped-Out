using infrastructure.Entities;
using infrastructure.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserCartController : ControllerBase
{
    private readonly UserProdService _userProdService;

    public UserCartController(UserProdService userProdService)
    {
        _userProdService = userProdService;
    }
    
    [HttpGet("/GetUserLikes")]
    public IActionResult GetUserLikes(int userId)
    {
        try
        {

            var productsList = _userProdService.GetUserLikes(userId);
        
            if (productsList == null)
            {
                return NotFound();
            }
            return Ok(productsList);

        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("/GetUserCartProducts")]
    public IActionResult GetUserCartProducts(int userId)
    {
        try
        {

            var productsList = _userProdService.GetUserCartProducts(userId);
            if (productsList == null)
            {
                return NotFound();
            }
            return Ok(productsList);

        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet]
    [Route("GetUserCart")]
    public ActionResult GetUserCartDetails(int userId)
    {
        try
        {
            var productsList = _userProdService.GetUserCartDetails(userId);
            if (productsList == null)
            {
                return NotFound();
            }
            return Ok(productsList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route("AddProductToUserCart")]
    public ActionResult AddProductToUserCart([FromBody] UserCart userCart)
    {
        try
        {
            if (userCart == null)
            {
                return BadRequest("Invalid request data");
            }
            
            int userId = userCart.UserId;
            int productId = userCart.ProductId;
            int colorId = userCart.ColorId;
            int sizeId = userCart.SizeId;
            int quantity = userCart.Quantity;

            _userProdService.AddProductToUserCart(userId, productId, colorId, sizeId, quantity);

            return CreatedAtAction(nameof(GetUserCartProducts), new { userId = userId });
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    [Route("RemoveProductFromCart")]
    public IActionResult RemoveProductFromCart(int userId, int productId, int colorId, int sizeId, int quantity)
    {
        try
        {
            _userProdService.RemoveProductFromCart(userId, productId, colorId, sizeId, quantity);
            return Ok("Product removed from cart successfully.");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut]
    [Route("UpdateProductQuantityInCart")]
    public IActionResult UpdateProductQuantityInCart(int userId, int productId, int colorId, int sizeId, int newQuantity)
    {
        try
        {
            _userProdService.UpdateProductQuantity(userId, productId, colorId, sizeId, newQuantity);
            return Ok("Product quantity updated successfully.");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost]
    [Route("AddProductToUserLikes")]
    public ActionResult AddProductToUserLikes([FromBody] UserLikes userLikes)
    {
        try
        {
            if (userLikes == null)
            {
                return BadRequest("Invalid request data");
            }

            int userId = userLikes.UserId;
            int productId = userLikes.ProductId;

            _userProdService.AddProductToUserLikes(userId, productId);

            return CreatedAtAction(nameof(GetUserLikes), new { userId = userId }, userLikes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "An error occurred while processing the request." });
        }
    }
    [HttpDelete]
    [Route("RemoveProductFromLiked")]
    public IActionResult RemoveProductFromLikes(int userId, int productId)
    {
        try
        {
            _userProdService.RemoveProductFromLikes(userId, productId);
            return Ok("Product removed from liked successfully.");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}