using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Ecom.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IMapper mapper, IUnitOfWork work) : base(mapper, work)
        {
        }
        //Display All Products
        [HttpGet("get-all")]
        public async Task<IActionResult> Get([FromQuery]ProductParams productParams)
        {

            try
            {
                //products with category and photos
                var products = await work.ProductRepository
                    .GetAllAsync(productParams);
                var totalCount = await work.ProductRepository.CountAsync();
                return Ok(new Pagination<ProductDTO>(productParams.PageNumber , productParams.pagesize , totalCount , products ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //Getting Product By ID
        [HttpGet("with-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                //products with category and photos
                var product = await work.ProductRepository
                    .GetByIdAsync(id , x=>x.Category , x=>x.Photos);
                var result = mapper.Map<ProductDTO>(product);
                if (result is null)
                    return BadRequest(new ResponseAPI(400));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("add-product")]
        public async Task<IActionResult> Add([FromForm] AddProductDTO ProductDto)
        {
            try
            {
                await work.ProductRepository.AddAsync(ProductDto);
                return Ok(new ResponseAPI(200,"Product Added Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400,ex.Message));
            }
        }

        [Authorize]
        [HttpPut("update-product")]
        public async Task<IActionResult> Update([FromForm] UpdateProductDTO ProductDto)
        {
            try
            {
                await work.ProductRepository.UpdateAsync(ProductDto);
                return Ok(new ResponseAPI(200, "Product Updated Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await work.ProductRepository
                    .GetByIdAsync(id ,x=> x.Photos, x =>x.Category);

                await work.ProductRepository.DeleteAsync(product);
                return Ok(new ResponseAPI(200, "Product Deleted Successfully") );
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }



    }
}
