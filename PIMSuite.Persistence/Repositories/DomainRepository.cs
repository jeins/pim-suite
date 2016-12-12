using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class DomainRepository : IDomainRepository
    {
        private DataContext _dataContext;

        public DomainRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Domain> GetDomains()
        {
            var domains = _dataContext.Domains;

            return domains;
        }

        public void InsertDomain(string DomainName)
        {
            var domain = new Domain
            {
                DomainName = DomainName
            };
            _dataContext.Domains.Add(domain);
            _dataContext.SaveChanges();
        }

        public void UpdateDomain(int DomainId, string DomainName)
        {
            var domain = _dataContext.Domains.SingleOrDefault(c => c.DomainId.Equals(DomainId));
            if (domain != null)
            {
                domain.DomainId = DomainId;
                _dataContext.SaveChanges();
            }
        }

        public void RemoveDomain(int DomainId)
        {
            var domain = _dataContext.Domains.FirstOrDefault(c => c.DomainId == DomainId);

            if (domain != null)
            {
                _dataContext.Domains.Remove(domain);
                _dataContext.SaveChanges();
            }
        }

        public Boolean isValid(string DomainName)
        {
            Domain domain = _dataContext.Domains.SingleOrDefault(c => c.DomainName == DomainName);
            if (domain != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
