using FluentValidation;
using FluentValidation.Results;

namespace Store.Sales.Domain
{
    public class Voucher
    {
        public string Code { get; private set; }
        public decimal? PercentageDiscount { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public TypeDiscountVoucher TypeDiscountVoucher { get; private set; }
        public int Quantity { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        public Voucher(string code, decimal? percentageDiscount, decimal? discountValue, int quantity,
            TypeDiscountVoucher typeDiscountVoucher, DateTime expirationDate, bool active, bool used)
        {
            Code = code;
            PercentageDiscount = percentageDiscount;
            DiscountValue = discountValue;
            Quantity = quantity;
            TypeDiscountVoucher = typeDiscountVoucher;
            ExpirationDate = expirationDate;
            Active = active;
            Used = used;
        }

        public ValidationResult ValidateIfApplicable()
        {
            return new VoucherApplicableValidation().Validate(this);
        }
    }

    public class VoucherApplicableValidation : AbstractValidator<Voucher>
    {
        public static string CodeErroMsg => "Voucher sem código válido.";
        public static string ExpirationDateErroMsg => "Este voucher está expirado.";
        public static string ActiveErroMsg => "Este voucher não é mais válido.";
        public static string UsedErroMsg => "Este voucher já foi used.";
        public static string QuantityErroMsg => "Este voucher não está mais disponível";
        public static string DiscountValueErroMsg => "O valor do desconto precisa ser superior a 0";
        public static string PercentageDiscountErroMsg => "O valor da porcentagem de desconto precisa ser superior a 0";

        public VoucherApplicableValidation()
        {
            RuleFor(c => c.Code)
                .NotEmpty()
                .WithMessage(CodeErroMsg);

            RuleFor(c => c.ExpirationDate)
                .Must(ExpirationDateHigherCurrent)
                .WithMessage(ExpirationDateErroMsg);

            RuleFor(c => c.Active)
                .Equal(true)
                .WithMessage(ActiveErroMsg);

            RuleFor(c => c.Used)
                .Equal(false)
                .WithMessage(UsedErroMsg);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QuantityErroMsg);

            When(f => f.TypeDiscountVoucher == TypeDiscountVoucher.Value, () =>
            {
                RuleFor(f => f.DiscountValue)
                    .NotNull()
                    .WithMessage(DiscountValueErroMsg)
                    .GreaterThan(0)
                    .WithMessage(DiscountValueErroMsg);
            });

            When(f => f.TypeDiscountVoucher == TypeDiscountVoucher.Percentage, () =>
            {
                RuleFor(f => f.PercentageDiscount)
                    .NotNull()
                    .WithMessage(PercentageDiscountErroMsg)
                    .GreaterThan(0)
                    .WithMessage(PercentageDiscountErroMsg);
            });
        }

        protected static bool ExpirationDateHigherCurrent(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }
}
