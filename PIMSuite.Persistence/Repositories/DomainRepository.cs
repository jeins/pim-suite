using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        // Constructors

        public DomainRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Fields

        private readonly DataContext _dataContext;

        // Methods

        public IEnumerable<Domain> GetDomains()
        {
            var domains = _dataContext.Domains;

            return domains;
        }

        public void InsertDomain(string domainName)
        {
            var domain = new Domain
            {
                DomainName = domainName
            };
            _dataContext.Domains.Add(domain);
            _dataContext.SaveChanges();
        }

        public void UpdateDomain(int domainId, string domainName)
        {
            var domain = _dataContext.Domains.SingleOrDefault(c => c.DomainId.Equals(domainId));
            if (domain != null)
            {
                domain.DomainId = domainId;
                _dataContext.SaveChanges();
            }
        }

        public void RemoveDomain(int domainId)
        {
            var domain = _dataContext.Domains.FirstOrDefault(c => c.DomainId == domainId);

            if (domain != null)
            {
                _dataContext.Domains.Remove(domain);
                _dataContext.SaveChanges();
            }
        }

        public bool CheckIsValid(string domainName)
        {
            Domain domain = _dataContext.Domains.SingleOrDefault(c => c.DomainName == domainName);
            if (domain != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Save()
        {
            _dataContext.SaveChanges();
        }
    }
}