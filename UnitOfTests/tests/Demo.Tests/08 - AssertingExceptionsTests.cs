namespace Demo.Tests
{
    public class AssertingExceptionsTests
    {
        [Fact]
        public void Calculator_Divide_MustReturnErrorDivisionByZero()
        {
            //Arrange
            var calculator = new Calculator();

            //Act & Assert 
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
        }

        [Fact]
        public void Employee_Salary_MustReturnErrorInferiorSalaryAllowed()
        {
            //Arrange & Act & Assert
            var exception = Assert.Throws<Exception>(()=> EmployeeFactory.Create("Karen", 300));

            
            Assert.Equal("Lower salary than allowed", exception.Message);
        }
    }
}
