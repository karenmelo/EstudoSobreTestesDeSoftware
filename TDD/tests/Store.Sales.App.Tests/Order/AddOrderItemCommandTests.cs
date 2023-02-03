using Store.Sales.App.Commands;

namespace Store.Sales.App.Tests.Order
{
    public class AddOrderItemCommandTests
    {
        [Fact(DisplayName = "Add Valid Command Item")]
        [Trait("Category", "Sales - Order Commands")]
        public void AddOrderItemCommand_CommandIsValid_MustPassValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Add Invalid Command Item")]
        [Trait("Category", "Sales - Order Commands")]
        public void AddOrderItemCommand_CommandIsInvalid_MustNotPassValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.ClientIdErroMsg, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.ProductIdErroMsg, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.NameErroMsg, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.QtdMinErroMsg, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.ValueErroMsg, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));

        }


        [Fact(DisplayName = "Add Valid Command Item")]
        [Trait("Category", "Sales - Order Commands")]
        public void AddOrderItemCommand_NumberOfUnitsHigherThanAllowed_MustNotPassValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", Store.Sales.Domain.Order.MAX_UNITS_ITEM + 1, 100);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.QtdMaxErroMsg, orderCommand.ValidationResult.Errors.Select(e => e.ErrorMessage));
        }
    }
}
