using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandidateAPI.Dto;

namespace CandidateAPI.Repositories
{
    public interface ICandidateRepository
    {
        CandidateDto GetCandidateByEmail(string email);
        void CreateCandidate(CandidateDto candidateDto);
        void UpdateCandidate(CandidateDto candidateDto);

    }
}