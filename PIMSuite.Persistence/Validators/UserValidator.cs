using FluentValidation;
using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).Must(BeUniqueEmail).WithMessage("Email already exists");
            RuleFor(x => x.Username).Must(BeUniqueUsername).WithMessage("Username already exists");
        }

        private bool BeUniqueUsername(string username)
        {
            return new DataContext().Users.FirstOrDefault(x => x.Username == username) == null;
        }

        private bool BeUniqueEmail(string email)
        {
            return new DataContext().Users.FirstOrDefault(x => x.Email == email) == null;
        }
    }
}
