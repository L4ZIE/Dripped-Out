using System.Collections.Generic;
using infrastructure.Entities;
using infrastructure.Repositories.Interface;

namespace service.Services
{
    public class ProductService
    {
        private readonly ICrud<Products> _productRepository;
        private readonly IProductMapper _productMapperRepository;

        public ProductService(ICrud<Products> productRepository, IProductMapper productMapperRepository)
        {
            _productRepository = productRepository;
            _productMapperRepository = productMapperRepository;
        }

        public Products getProductById(int productId)
        {
            return _productRepository.Read(productId);
        }

        public void createProduct(Products products)
        {
            _productRepository.Create(products);
        }

        public void updateProduct(Products products)
        {
            _productRepository.Update(products);
        }

        public void deleteProduct(int productId)
        {
            _productRepository.Delete(productId);
        }

        public IEnumerable<Products> getAllProducts()
        {
            return _productMapperRepository.getAllProducts();
        }

        public List<Products> getProductByType(int typeId)
        {
            return _productMapperRepository.getProductbyType(typeId);
        }

        public List<Products> getProductByGenderType(int typeId, string gender)
        {
            return _productMapperRepository.getProductbyGenderType(typeId, gender);
        }

        public List<Products> getProductByGender(string gender)
        {
            return _productMapperRepository.getProductbyGender(gender);
        }
    }
}