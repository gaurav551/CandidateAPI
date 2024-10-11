using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidateAPI.Models
{
    public class Candidate
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BestCallTime { get; set; }
        public string LinkedinProfile { get; set; }
        public string GithubProfile { get; set; }
        public string Comment { get; set; }

    }
}