using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EnterpriseAPI.Business;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/creators")]
    public class CreatorsController : ControllerBase
    {
        private readonly ICreatorsBusiness _creatorsBusiness;
        private string zeroIdParameterError = "The parameter should be greater than zero";
        private string emptyParameterError = "The parameter should not be empty";

        public CreatorsController(ICreatorsBusiness creatorsBusiness)
        {
            _creatorsBusiness = creatorsBusiness;
        }

        [HttpGet("getAll")]
        public ActionResult Get()
        {
            var creators = _creatorsBusiness.GetAll();
            if (creators == null)
            {
                return NoContent();
            }


            return Ok(creators);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest(zeroIdParameterError);
            }

            var creator = _creatorsBusiness.GetCreatorByID(id);

            if (creator == null)
            {
                return NotFound();
            }

            return Ok(creator);
        }

        [HttpPost]
        public async Task<ActionResult> AddCreator(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(emptyParameterError);
            }

            if (await _creatorsBusiness.AddCreator(name))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(zeroIdParameterError);
            }

            if (await _creatorsBusiness.DeleteCreator(id))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
