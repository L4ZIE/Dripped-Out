using infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using service.Services;

namespace api.Controllers;

    [ApiController] 
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("api/Products")]
        public IEnumerable<Products> getAllProducts()
        {
            return _productService.getAllProducts();
        }

        [HttpGet]
        [Route("/api/product/{productId}")]
        public ActionResult<Products> GetProductById(int productId)
        {
            var product = _productService.getProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Route("/api/product/create")]
        public ActionResult CreateProduct([FromBody] Products product)
        {
            if (product == null)
            {
                return BadRequest("Invalid product data");
            }

            _productService.createProduct(product);

            return CreatedAtAction(nameof(GetProductById), new { productId = product.ProductId }, product);
        }

        [HttpPut]
        [Route("/api/product/update/{productId}")]
        public ActionResult UpdateProduct(int productId, [FromBody] Products product)
        {
            if (product == null || productId != product.ProductId)
            {
                return BadRequest("Invalid product data");
            }

            var existingProduct = _productService.getProductById(productId);

            if (existingProduct == null)
            {
                return NotFound();
            }

            _productService.updateProduct(product);

            return NoContent();
        }

        [HttpDelete]
        [Route("/api/product/delete/{productId}")]
        public ActionResult DeleteProduct(int productId)
        {
            var existingProduct = _productService.getProductById(productId);

            if (existingProduct == null)
            {
                return NotFound();
            }

            _productService.deleteProduct(productId);

            return NoContent();
        }

        [HttpGet]
        [Route("/api/product/type/{typeId}")]
        public ActionResult<List<Products>> GetProductsByType(int typeId)
        {
            var products = _productService.getProductByType(typeId);
            return Ok(products);
        }

        [HttpGet]
        [Route("/api/product/gender/{gender}")]
        public ActionResult<List<Products>> GetProductsByGender(string gender)
        {
            var products = _productService.getProductByGender(gender);
            return Ok(products);
        }

        [HttpGet]
        [Route("api/product/type/{typeId}/gender/{gender}")]
        public ActionResult<List<Products>> GetProductsByGenderType(int typeId, string gender)
        {
            var products = _productService.getProductByGenderType(typeId, gender);
            return Ok(products);
        }
    }
