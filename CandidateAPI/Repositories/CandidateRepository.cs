using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandidateAPI.Data;
using CandidateAPI.Dto;
using CandidateAPI.Models;
using CandidateAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CandidateAPI.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public CandidateDto GetCandidateByEmail(string email)
        {
            var candidate = _context.Candidates.FirstOrDefault(c => c.Email == email);
            return candidate != null ? MapToDto(candidate) : null;
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