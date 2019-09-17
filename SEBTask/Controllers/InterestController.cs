using API.Models;
using Domain.Contracts.Services;
using Domain.Entities;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/users/{userId}")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly IInterestRateService interestRateService;

        public InterestController(IInterestRateService interestRateService)
        {
            this.interestRateService = interestRateService;
        }

        /// <summary>
        /// Evaluates how the change of the base rate will change the interest rate
        /// </summary>
        /// <response code="404">The user with specific agreement was not found</response>
        /// <response code="503">Integration with www.lb.lt failed while trying to retrieve the latest Vilib rate</response>  
        [HttpGet("agreements/{agreementId}/ImpactOfBaseRateChange")]
        [ProducesResponseType(typeof(ImpactOfBaseRateChange), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<ActionResult> CalculateBaseRateChangeImpact(
            [FromRoute] long userId,
            [FromRoute] long agreementId,
            [FromQuery][Required] BaseRateCodeEnum newBaseRate
        )
        {
            return Ok(await this.interestRateService.CalculateImpactOfBaseRateChange(userId, agreementId, newBaseRate));
        }
    }
}