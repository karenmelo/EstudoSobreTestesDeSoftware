namespace Demo.Tests
{
    public class AssertingObjectTypesTests
    {
        [Fact]
        public void EmployeeFactory_Create_MustReturnEmployeeType()
        {
            //Arrange & Act
            var employee = EmployeeFactory.Create("Karen", 50000);

            //Assert
            Assert.IsType<Employee>(employee);
        }

        [Fact]
        public void EmployeeFactory_Create_MustReturnTypeDerivedPerson()
        {
            //Arrange & Act
            var employee = EmployeeFactory.Create("Karen", 50000);

            //Assert
            Assert.IsAssignableFrom<Person>(employee);
        }
    }
}
