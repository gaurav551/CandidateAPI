using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandidateAPI.Dto;
using CandidateAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CandidateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : Controller
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPut]
        public IActionResult CreateOrUpdateCandidate(CandidateDto candidateDto)
        {
            return Ok();
           
        }

    }
}