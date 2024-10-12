using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandidateAPI.Dto;
using CandidateAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CandidateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidateController : Controller
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateController(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        [HttpPut]
        public IActionResult CreateOrUpdateCandidate(CandidateDto candidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var existingCandidate = _candidateRepository.GetCandidateByEmail(candidateDto.Email);

                if (existingCandidate != null)
                {
                    // If candidate exists, update their information
                    _candidateRepository.UpdateCandidate(candidateDto);
                    return Ok("Candidate updated successfully.");
                }
                else
                {
                    // If candidate does not exist, create a new one
                    _candidateRepository.CreateCandidate(candidateDto);
                    return StatusCode(201, "Candidate created successfully."); 

                }

            }
            catch (Exception ex)
            {
                //we can log the exception
                return StatusCode(500, "An unexpected error occurred.");
            }

        }

    }
}