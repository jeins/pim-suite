using System;
using System.Collections.Generic;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public interface IDomainRepository
    {
        IEnumerable<Domain> GetDomains();
        void InsertDomain(string domainName);
        void UpdateDomain(int domainId, string domainName);
        void RemoveDomain(int domainId);
        bool CheckIsValid(string domainName);
        void Save();
    }
}