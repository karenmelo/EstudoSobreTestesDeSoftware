namespace Demo
{
    public class Person
    {
        public string Name { get; protected set; }
        public string Surname { get; set; }
    }

    public class Employee : Person
    {
        public double Salary { get; private set; }
        public ProfessionalLevel ProfessionalLevel { get; private set; }
        public IList<string> Skills { get; private set; }

        public Employee(string name, double salary)
        {
            Name = string.IsNullOrEmpty(name) ? "Fulano" : name;
            SetSalary(salary);
            SetSkills();
        }

        public void SetSalary(double salary)
        {
            if (salary < 500) throw new Exception("Lower salary than allowed");

            Salary = salary;
            if (salary < 2000) ProfessionalLevel = ProfessionalLevel.Junior;
            else if (salary >= 2000 && salary < 8000) ProfessionalLevel = ProfessionalLevel.Intermediate;
            else if (salary >= 8000) ProfessionalLevel = ProfessionalLevel.Senior;
        }

        private void SetSkills()
        {
            var basicSkills = new List<string>()
            {

                "Programming logic",
                "OOP"
            };

            Skills = basicSkills;

            switch (ProfessionalLevel)
            {
                case ProfessionalLevel.Intermediate:
                    Skills.Add("Tests");
                    break;
                case ProfessionalLevel.Senior:
                    Skills.Add("Tests");
                    Skills.Add("Microservices");
                    break;
            }
        }
    }

    public enum ProfessionalLevel
    {
        Junior,
        Intermediate,
        Senior
    }

    public class EmployeeFactory
    {
        public static Employee Create(string name, double salary)
        {
            return new Employee(name, salary);
        }
    }
}
