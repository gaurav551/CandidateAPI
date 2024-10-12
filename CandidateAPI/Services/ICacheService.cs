using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CandidateAPI.Services
{
    public interface ICacheService
    {
        void Set<T>(string key, T value, TimeSpan expiration);
        T Get<T>(string key);
        void Remove(string key);
    }
}