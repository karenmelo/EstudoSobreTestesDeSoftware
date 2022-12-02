using Features.Clients;
using FluentValidation;
using System;

namespace Features.Validators
{
    public class ClientValidator : AbstractValidator<Client>
    {
        public ClientValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Por favor, certifique-se de ter inserido o nome")
                .Length(2, 150).WithMessage("O nome deve ter entre 2 e 150 caracteres");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("Por favor, certifique-se de ter inserido o sobrenome")
                .Length(2, 150).WithMessage("O sobrenome deve ter entre 2 e 150 caracteres");

            RuleFor(c => c.Birthdate)
                .NotEmpty()
                .Must(HaveMinimumAge)
                .WithMessage("O cliente deve ter 18 anos ou mais");

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }

        public static bool HaveMinimumAge(DateTime birthdate)
        {
            return birthdate <= DateTime.Now.AddYears(-18);
        }
    }
}
