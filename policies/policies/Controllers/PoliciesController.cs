using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using policies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using policies.Services;
namespace policies.Controllers
{
    [ApiController]
    [Route("api/policies")]
    [Authorize]
    public class PoliciesController : ControllerBase
    {
        //public PoliciesService _policiesService;

        private readonly IPoliciesService _policiesService;

        public PoliciesController(IPoliciesService policiesService)
        {
            _policiesService = policiesService;
        }


        [HttpGet]
        public ActionResult<List<Policy>> Get()
        {
            var policies = _policiesService.Get();
            return Ok(policies);
        }

        [HttpPost]
        public ActionResult CreatePolicy([FromBody] Policy policy)
        {
            try
            {
                _policiesService.Create(policy);

                return CreatedAtRoute("GetPolicy", new { id = policy.Id }, policy);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{plateOrNumber}")]
        public ActionResult<List<Policy>> GetPolicyByPlateOrNumber(string plateOrNumber)
        {
            var policies = _policiesService.GetPolicyByPlateOrNumber(plateOrNumber);

            if (policies.Count == 0)
            {
                return BadRequest(new { message = "No policies found for the provided plate or policy number." });
            }

            return Ok(policies);
        }
    }
}
