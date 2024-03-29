﻿using Azure;
using Mango.Services.ProductAPI.Models.Dtos;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDto _responseDto;
        private IProductRepository _productRepository;
        public ProductAPIController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            this._responseDto = new ResponseDto();
        }
        [HttpGet]
		public async Task<object> Get()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await _productRepository.GetProducts();
                _responseDto.Result = productDtos;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                ProductDto productDto = await _productRepository.GetProductById(id);
                _responseDto.Result = productDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }
        [HttpPost]
		[Authorize]
		public async Task<object> Post([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto model = await _productRepository.CreateUpdateProduct(productDto);
                _responseDto.Result = model;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }
        [HttpPut]
		[Authorize]
		public async Task<object> Put([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto model = await _productRepository.CreateUpdateProduct(productDto);
                _responseDto.Result = model;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }
        [HttpDelete]
		[Authorize(Roles = "Admin")]
		[Route("{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool isSuccess = await _productRepository.DeleteProduct(id);
                _responseDto.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _responseDto;
        }
    }
}
