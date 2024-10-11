using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandidateAPI.Data;
using CandidateAPI.Dto;
using CandidateAPI.Models;
using CandidateAPI.Repositories;
using CandidateAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cacheService;
        public CandidateRepository(ApplicationDbContext context, ICacheService cacheService)
        {
            _cacheService = cacheService;
            _context = context;
        }

        public CandidateDto GetCandidateByEmail(string email)
        {
            var cachedCandidate = _cacheService.Get<CandidateDto>(email);
            if(cachedCandidate!=null)
            {
                //from cache memory;
                return cachedCandidate;
            }
            //if not found in cache, query from database

            var candidate = _context.Candidates.FirstOrDefault(c => c.Email == email);
            if(candidate != null)
            {
                var candidateDto = MapToDto(candidate);
                //set item to cache
                _cacheService.Set(email,candidateDto,TimeSpan.FromMinutes(20)); // cache for 20 mins
                return candidateDto;
            }
            return null;
        }

        public void CreateCandidate(CandidateDto candidateDto)
        {
            var newCandidate = new Candidate
            {
                FirstName = candidateDto.FirstName,
                LastName = candidateDto.LastName,
                PhoneNumber = candidateDto.PhoneNumber,
                Email = candidateDto.Email,
                BestCallTime = candidateDto.BestCallTime,
                LinkedInProfileURL = candidateDto.LinkedInProfileURL,
                GitHubProfileURL = candidateDto.GitHubProfileURL,
                Comment = candidateDto.Comment
            };

            _context.Candidates.Add(newCandidate);
            _context.SaveChanges();
            //Add newly added item to cache
            _cacheService.Set(candidateDto.Email, candidateDto,TimeSpan.FromMinutes(20));
        }

        public void UpdateCandidate(CandidateDto candidateDto)
        {
            var existingCandidate = _context.Candidates
                .FirstOrDefault(c => c.Email == candidateDto.Email);

            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidateDto.FirstName;
                existingCandidate.LastName = candidateDto.LastName;
                existingCandidate.PhoneNumber = candidateDto.PhoneNumber;
                existingCandidate.BestCallTime = candidateDto.BestCallTime;
                existingCandidate.LinkedInProfileURL = candidateDto.LinkedInProfileURL;
                existingCandidate.GitHubProfileURL = candidateDto.GitHubProfileURL;
                existingCandidate.Comment = candidateDto.Comment;

                _context.Candidates.Update(existingCandidate);
                _context.SaveChanges();
                //update the cache as well
                _cacheService.Set(candidateDto.Email, candidateDto,TimeSpan.FromMinutes(20));

            }
        }
        //Mapper to map for Candidate to CandidateDTO

        private CandidateDto MapToDto(Candidate candidate)
        {
            return new CandidateDto
            {
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                PhoneNumber = candidate.PhoneNumber,
                Email = candidate.Email,
                BestCallTime = candidate.BestCallTime,
                LinkedInProfileURL = candidate.LinkedInProfileURL,
                GitHubProfileURL = candidate.GitHubProfileURL,
                Comment = candidate.Comment
            };
        }

        
    }
}