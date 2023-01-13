namespace Store.Sales.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validate Voucher Type Value Valid")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVoucherTypeValue_MustBeValid()
        {
            // Arrange
            var voucher = new Voucher
          (
                "PROMO15",
                15,
                null,
                VoucherDiscountType.Value,
                1,
                DateTime.Now.AddDays(15),
                true,
                false
            );

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            Assert.True(result.IsValid);
        }


        [Fact(DisplayName = "Validate Voucher Type Value Invalid")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVoucherTypeValue_MustBeInvalid()
        {
            // Arrange
            var voucher = new Voucher("", null, null, VoucherDiscountType.Value, 0, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherApplicableValidation.ActiveErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.CodeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.ExpirationDateErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.QuantityErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.UsedErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.DiscountValueErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Validate Voucher Type Percentage Valid")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVoucherTypePercentage_MustBeValid()
        {
            // Arrange
            var voucher = new Voucher("PROMO1OFF", null, 10, VoucherDiscountType.Percentage, 1, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            Assert.True(result.IsValid);

        }

        [Fact(DisplayName = "Validate Voucher Type Percentage Invalid")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVoucherTypePercentage_MustBeInvalid()
        {
            // Arrange
            var voucher = new Voucher("", 15, null, VoucherDiscountType.Percentage, 0, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidateIfApplicable();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherApplicableValidation.ActiveErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.CodeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.ExpirationDateErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.QuantityErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.UsedErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.DiscountPercentualErroMsg, result.Errors.Select(c => c.ErrorMessage));

        }
    }
}
