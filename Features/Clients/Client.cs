using Features.Core;
using Features.Validators;

namespace Features.Clients
{
    public class Client : Entity
    {
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public DateTime Birthdate { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public string Email { get; private set; }
        public bool Active { get; private set; }

        protected Client()
        {
        }
        
        public Client(string name, string lastName, DateTime birthdate, DateTime registrationDate, string email, bool active)
        {
            Name = name;
            LastName = lastName;
            Birthdate = birthdate;
            RegistrationDate = registrationDate;
            Email = email;
            Active = active;
        }

        public string FullName()
        {
            return $"{Name} {LastName}";
        }

        public bool IsSpecial()
        {
            return RegistrationDate < DateTime.Now.AddYears(-3) && Active;
        }

        public void Disable()
        {
            Active = false;
        }

        public override bool IsValid()
        {
            ValidationResult = new ClientValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }    
}
