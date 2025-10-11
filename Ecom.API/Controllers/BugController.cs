using AutoMapper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecom.API.Controllers
{
    public class BugController : BaseController
    {
        public BugController(IMapper mapper, IUnitOfWork work) : base(mapper, work)
        {
        }
    
        [HttpGet("not-found")]
        public async Task<ActionResult> GetNotFound()
        {
            var thing =await work.ProductRepository.GetByIdAsync(100);
            if (thing == null) return NotFound();
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public async Task<ActionResult> GetServerError()
        {
            var thing = await work.ProductRepository.GetByIdAsync(100);
            thing.Name = ""; // this will cause a server error if Name is required
            return Ok(thing);
        } 

        [HttpGet("bad-request/{Id}")]
        public async Task<ActionResult> GetBadRequest(int id)
        {
            return BadRequest();
        }

        [HttpGet("bad-request")]
        public async Task<ActionResult> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
