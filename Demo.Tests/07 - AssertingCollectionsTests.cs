namespace Demo.Tests
{
    public class AssertingCollectionsTests
    {
        [Fact]
        public void Employee_Skills_MustNotPossessEmptySkills()
        {
            //Arrange & Act
            var employee = EmployeeFactory.Create("Karen", 50000);

            //Assert
            Assert.All(employee.Skills, skills => Assert.False(string.IsNullOrEmpty(skills)));
        }

        [Fact]
        public void Employee_Skills_JuniorMustOwnSkillBasic()
        {
            //Arrange & Act
            var employee = EmployeeFactory.Create("Karen", 50000);

            //Assert
            Assert.Contains("OOP", employee.Skills);
        }

        [Fact]
        public void Employee_Skills_JuniorMustNotPossessAdvancedSkill()
        {
            //Arrange & Act
            var employee = EmployeeFactory.Create("Karen", 50000);

            //Assert
            Assert.Contains("Microservice", employee.Skills);
        }

        [Fact]
        public void Employee_Skills_SeniorMustPossessAllSkills()
        {
            //Arrange & Act
            var employee = EmployeeFactory.Create("Karen", 50000);

            var skills = new[]
            {
                "Programming logic",
                "OOP",
                "Tests",
                "Microservices"
            };

            //Assert
            Assert.Equal(skills, employee.Skills);
        }
    }
}
