namespace Demo.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Calculator_Sum_ReturnSumValue()
        {
            //Arrange - Objeto preparado para ser usado na acao
            var calculator = new Calculator();

            //Act - acao a ser executada
            var result = calculator.Sum(2, 2);

            //Assert - o resultado esperado
            Assert.Equal(4, result);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 2, 4)]
        [InlineData(3, 3, 6)]
        [InlineData(4, 2, 6)]
        [InlineData(7, 3, 10)]
        [InlineData(8, 2, 10)]
        [InlineData(9, 9, 18)]
        public void Calculator_Sum_ReturnCorrectSumValues(double value1, double value2, double total)
        {
            //Arrange
            var calculator = new Calculator();

            //Act
            var result = calculator.Sum(value1, value2);

            //Assert
            Assert.Equal(total, result);
        }
    }
}
