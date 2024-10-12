using CandidateAPI.Controllers;
using CandidateAPI.Dto;
using CandidateAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;
namespace CandidateAPI.Tests
{
    public class CandidateControllerTests
    {
        private readonly Mock<ICandidateRepository> _mockRepository;
        private readonly CandidateController _controller;
        public CandidateControllerTests()
        {
            _mockRepository = new Mock<ICandidateRepository>();
            _controller = new CandidateController(_mockRepository.Object);
        }
        [Fact]
        public void CreateOrUpdateCandidate_ValidCandidate_CreatesCandidate()
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
            _mockRepository.Setup(r => r.GetCandidateByEmail(candidateDto.Email)).Returns((CandidateDto)null);

            var result = _controller.CreateOrUpdateCandidate(candidateDto) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            _mockRepository.Verify(r => r.CreateCandidate(candidateDto), Times.Once);
        }
        [Fact]
        public void CreateOrUpdateCandidate_ExistingCandidate_UpdatesCandidate()
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
                Comment = "Hi i am robert"
            };
            _mockRepository.Setup(r => r.GetCandidateByEmail(candidateDto.Email)).Returns(candidateDto);

            var result = _controller.CreateOrUpdateCandidate(candidateDto) as OkResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            _mockRepository.Verify(r => r.UpdateCandidate(candidateDto), Times.Once);
        }
        [Fact]
        public void CreateOrUpdateCandidate_InvalidModel()
        {

            _controller.ModelState.AddModelError("Email", "Email is required.");
            var candidateDto = new CandidateDto(); // Empty data

            var result = _controller.CreateOrUpdateCandidate(candidateDto) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
        [Fact]
        public void CreateOrUpdateCandidate_ExceptionThrown()
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
                Comment = "Hi i am robert"
            };
            _mockRepository.Setup(r => r.GetCandidateByEmail(candidateDto.Email)).Throws(new Exception("Database error"));

            var result = _controller.CreateOrUpdateCandidate(candidateDto) as ObjectResult;
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
        }
    }
}
