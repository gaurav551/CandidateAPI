using System;
using System.Linq;
using CandidateAPI.Data;
using CandidateAPI.Dto;
using CandidateAPI.Models;
using CandidateAPI.Repositories;
using CandidateAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CandidateAPI.Tests.Repositories
{
    public class CandidateRepositoryTests
    {
        private readonly CandidateRepository _repository;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly ApplicationDbContext _dbContext;

        public CandidateRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CandidateDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _cacheServiceMock = new Mock<ICacheService>();
            _repository = new CandidateRepository(_dbContext, _cacheServiceMock.Object);
        }

        [Fact]
        public void GetCandidateByEmail_CandidateExistsInCache_ReturnsCandidate()
        {
            var candidateDto = new CandidateDto
            {
                FirstName = "Robert",
                LastName = "will",
                PhoneNumber = "1234567890",
                Email = "robert.11@gmail.com",
                BestCallTime = "10:00 AM - 12:00 PM",
                LinkedInProfileURL = "https://linkedin.com/in/robert",
                GitHubProfileURL = "https://github.com/robert",
                Comment = "Test comment"
            };

            _cacheServiceMock.Setup(c => c.Get<CandidateDto>(candidateDto.Email))
                .Returns(candidateDto);
            var result = _repository.GetCandidateByEmail(candidateDto.Email);
            Assert.NotNull(result);
            Assert.Equal(candidateDto.Email, result.Email);
            _cacheServiceMock.Verify(c => c.Get<CandidateDto>(candidateDto.Email), Times.Once);
        }

        [Fact]
        public void GetCandidateByEmail_CandidateNotInCache()
        {
            var candidate = new Candidate
            {
                FirstName = "Robert",
                LastName = "will",
                PhoneNumber = "1234567890",
                Email = "robert.11@gmail.com",
                BestCallTime = "10:00 AM - 12:00 PM",
                LinkedInProfileURL = "https://linkedin.com/in/robert",
                GitHubProfileURL = "https://github.com/robert",
                Comment = "Test comment"
            };

            _dbContext.Candidates.Add(candidate);
            _dbContext.SaveChanges();
            var result = _repository.GetCandidateByEmail(candidate.Email);
            Assert.NotNull(result);
            Assert.Equal(candidate.Email, result.Email);
            _cacheServiceMock.Verify(c => c.Set(candidate.Email, It.IsAny<CandidateDto>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public void CreateCandidate_AddsCandidateToDatabaseAndCache()
        {
            var candidateDto = new CandidateDto
            {
                FirstName = "Alice",
                LastName = "Smith",
                PhoneNumber = "1112223333",
                Email = "alice.smith@example.com",
                BestCallTime = "Afternoon",
                LinkedInProfileURL = "http://linkedin.com/in/alicesmith",
                GitHubProfileURL = "http://github.com/alicesmith",
                Comment = "New candidate"
            };
            _repository.CreateCandidate(candidateDto);
            var savedCandidate = _dbContext.Candidates.FirstOrDefault(c => c.Email == candidateDto.Email);
            Assert.NotNull(savedCandidate);
            Assert.Equal(candidateDto.FirstName, savedCandidate.FirstName);
            Assert.Equal(candidateDto.LastName, savedCandidate.LastName);
            _cacheServiceMock.Verify(c => c.Set(candidateDto.Email, candidateDto, It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public void UpdateCandidate_UpdatesExistingCandidate()
        {
            var candidate = new Candidate
            {
                FirstName = "Bob",
                LastName = "Johnson",
                PhoneNumber = "4445556666",
                Email = "bob.johnson@example.com",
                BestCallTime = "Morning",
                LinkedInProfileURL = "http://linkedin.com/in/bobjohnson",
                GitHubProfileURL = "http://github.com/bobjohnson",
                Comment = "Old candidate"
            };

            _dbContext.Candidates.Add(candidate);
            _dbContext.SaveChanges();

            var updatedCandidateDto = new CandidateDto
            {
                FirstName = "Bobby",
                LastName = "Johnson",
                PhoneNumber = "4445556666",
                Email = candidate.Email,
                BestCallTime = "Afternoon",
                LinkedInProfileURL = "http://linkedin.com/in/bobbyjohnson",
                GitHubProfileURL = "http://github.com/bobbyjohnson",
                Comment = "Updated candidate"
            };

            _repository.UpdateCandidate(updatedCandidateDto);
            var updatedCandidate = _dbContext.Candidates.FirstOrDefault(c => c.Email == candidate.Email);
            Assert.NotNull(updatedCandidate);
            Assert.Equal(updatedCandidateDto.FirstName, updatedCandidate.FirstName);
            Assert.Equal(updatedCandidateDto.LinkedInProfileURL, updatedCandidate.LinkedInProfileURL);
            _cacheServiceMock.Verify(c => c.Set(updatedCandidateDto.Email, updatedCandidateDto, It.IsAny<TimeSpan>()), Times.Once);
        }
    }
}
