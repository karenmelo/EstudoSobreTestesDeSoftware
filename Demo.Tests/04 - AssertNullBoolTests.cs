namespace Demo.Tests
{
    public class AssertNullboolTests
    {
        [Fact]
        public void Employee_Name_MustNotBeNullOrEmpty()
        {
            //Arrange
            var employee = new Employee("", 50000);

            //Assert
            Assert.False(string.IsNullOrEmpty(employee.Name));
        }

        [Fact]
        public void Employee_Surname_MustNotHaveSurname()
        {
            //Arrange & Act
            var employee = new Employee("Karen", 50000);

            //Assert
            Assert.Null(employee.Surname);
            Assert.True(string.IsNullOrEmpty(employee.Surname));
            Assert.False(employee.Surname?.Length > 0);
        }   
    }
}
