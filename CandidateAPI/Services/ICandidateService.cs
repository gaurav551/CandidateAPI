using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandidateAPI.Dto;

namespace CandidateAPI.Services
{
    public interface ICandidateService
    {
        CandidateDto GetCandidateByEmail(string email);
        void CreateCandidate();
        void UpdateCandidate();

    }
}