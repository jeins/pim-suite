using FluentValidation;
using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Validators
{
    public class DomainValidator : AbstractValidator<Domain>
    {
        public DomainValidator()
        {
            RuleFor(x => x.DomainName).Must(BeUniqueDomain).WithMessage("Domain already exists");
        }

        private bool BeUniqueDomain(string domainname)
        {
            return new DataContext().Domains.FirstOrDefault(x => x.DomainName == domainname) == null;
        }
       
    }
}
