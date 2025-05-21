using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Business;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsBusiness _requestsBusiness;

        public RequestsController(IRequestsBusiness requestsBusiness)
        {
            _requestsBusiness = requestsBusiness;
        }

        // GET: api/<RequestsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RequestsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RequestsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RequestsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RequestsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
