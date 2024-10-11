using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CandidateAPI.Dto
{
    public class CandidateDto
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

       
        public string Email { get; set; }

        public string BestCallTime { get; set; }

        public string LinkedInProfileURL { get; set; }

      
        public string GitHubProfileURL { get; set; }

        public string Comments { get; set; }



    }
}