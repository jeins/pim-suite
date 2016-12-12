using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public interface IDomainRepository
    {
        IEnumerable<Domain> GetDomains();
        void InsertDomain(string DomainName);
        void UpdateDomain(int DomainId, string DomainName);
        void RemoveDomain(int DomainId);
        Boolean isValid(string DomainName);
    }
}
