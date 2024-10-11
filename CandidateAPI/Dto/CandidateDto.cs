using CandidateAPI.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CandidateAPI.Dto
{
    public class CandidateDto
    {

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public string BestCallTime { get; set; }

        [RegularExpression(RegexPatterns.LinkedInProfileUrl, ErrorMessage = "Invalid LinkedIn profile URL.")]
        public string LinkedInProfileURL { get; set; }

        [RegularExpression(RegexPatterns.GitHubProfileUrl, ErrorMessage = "Invalid GitHub profile URL.")]
        public string GitHubProfileURL { get; set; }

        public string Comment { get; set; }



    }
}