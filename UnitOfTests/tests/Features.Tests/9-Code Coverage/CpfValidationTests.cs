using Features.Core;
using FluentAssertions;

namespace Features.Tests
{
    public class CpfValidationTests
    {
        [Theory(DisplayName = "CPF Valid")]
        [Trait("Category", "CPF Validation Tests")]
        [InlineData("56245650038")]
        [InlineData("67857110027")]
        [InlineData("26197024020")]
        [InlineData("01489184090")]
        [InlineData("13830803800")]
        public void Cpf_ValidateMultipleNumbers_AllMustBeValid(string cpf)
        {
            //Arrange
            var cpfValidation = new CpfValidation();

            //Act & Assert
            cpfValidation.IsValid(cpf).Should().BeTrue();
        }

        [Theory(DisplayName = "CPF Valid")]
        [Trait("Category", "CPF Validation Tests")]
        [InlineData("5624565003811")]
        [InlineData("6785711002711")]
        [InlineData("26197024d02023")]
        [InlineData("014891840d90455")]
        public void Cpf_ValidateMultipleNumbers_AllMustBeInvalid(string cpf)
        {
            //Arrange
            var cpfValidation = new CpfValidation();

            //Act & Assert
            cpfValidation.IsValid(cpf).Should().BeFalse();
        }

    }
}
