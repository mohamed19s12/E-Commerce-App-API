using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class BasketsController : BaseController
    {
        public BasketsController(IMapper mapper, IUnitOfWork work) : base(mapper, work)
        {
        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var basket = await work.CustomerBasketRepository.GetBasketAsync(id);
            if(basket != null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(basket);
        }
        [HttpPost("update-basket")]
        public async Task<IActionResult> add(CustomerBasket basket)
        {
            var _basket = work.CustomerBasketRepository.UpdateBasketAsync(basket);
            return Ok(_basket);
        }
        [HttpDelete("delete-basket/{id}")]
        public async Task<IActionResult> delete(string id)
        {
            var result =await work.CustomerBasketRepository.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200 , "Item Deleted")) 
                : BadRequest(new ResponseAPI(400));
        }
    }
}
