﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EnterpriseAPI.Business;
using EnterpriseAPI.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/requests")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsBusiness _requestsBusiness;
        private string zeroIdParameterError = "The parameter should be greater than zero";

        public RequestsController(IRequestsBusiness requestsBusiness)
        {
            _requestsBusiness = requestsBusiness;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddRequest([FromBody] RequestParameters parameters)
        {
            if (parameters == null)
            {
                return BadRequest("Required parameters are missing");
            }

            if (!DateTime.TryParseExact(parameters.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime formattedDate))
            {
                return BadRequest("Invalid date format");
            };

            var request = new Request()
            {
                CreatorId = parameters.CreatorId,
                Date = formattedDate,
                Type = (Models.Type)parameters.Type,
                GrossValue = parameters.Value
            };

            if (await _requestsBusiness.CreateRequest(request))
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
        }

        [HttpPost("approve/{id}")]
        public async Task<ActionResult> Approve(int id)
        {
            if (id <= 0)
            {
                return BadRequest(zeroIdParameterError);
            }

            if (await _requestsBusiness.ApproveRequest(id))
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
        }
 
        [HttpPost("deny/{id}")]
        public async Task<ActionResult> Deny(int id)
        {
            if (id <= 0)
            {
                return BadRequest(zeroIdParameterError);
            }

            if (await _requestsBusiness.DenyRequest(id))
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
        }

        [HttpGet("getRequestsByCreatorId/{creatorId}")]
        public ActionResult GetAllByCreatorId(int creatorId)
        {
            if (creatorId <= 0)
            {
                return BadRequest(zeroIdParameterError);
            }

            var requests = _requestsBusiness.GetRequestsByCreatorId(creatorId);

            if (requests == null || !requests.Any())
            {
                return NotFound();
            }

            return Ok(requests);
        }
    }

    public class RequestParameters
    {
        [JsonPropertyName("creator_id")]
        public int CreatorId { get; set; }
        [JsonPropertyName("value")]
        public decimal Value { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; }
    }
}
