using FluentValidation;
using FluentValidation.Results;

namespace Store.Sales.Domain
{
    public class Voucher
    {
        public Voucher(string code, decimal? discountValue, decimal? discountPercentual, VoucherDiscountType voucherDiscountType, int quantity, DateTime expirationDate, bool active, bool used)
        {
            Code = code;
            DiscountValue = discountValue;
            DiscountPercentual = discountPercentual;
            VoucherDiscountType = voucherDiscountType;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            Active = active;
            Used = used;
        }

        public string Code { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public decimal? DiscountPercentual { get; private set; }
        public VoucherDiscountType VoucherDiscountType { get; private set; }
        public int Quantity { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        public ValidationResult ValidateIfApplicable()
        {
            return new VoucherApplicableValidation().Validate(this);
        }
    }

    public class VoucherApplicableValidation : AbstractValidator<Voucher>
    {
        public static string CodeErroMsg => "Voucher sem codigo valido.";
        public static string ExpirationDateErroMsg => "Este voucher esta expirado";
        public static string ActiveErroMsg => "Este voucher nao e mais valido.";
        public static string UsedErroMsg => "Este voucher ja foi utilizado.";
        public static string QuantityErroMsg => "Estou voucher nao esta mais disponivel.";
        public static string DiscountValueErroMsg => "O valor do desconto precisa ser superior a 0.";
        public static string DiscountPercentualErroMsg => "O valor da porcentagem do desconto precisa ser superior a 0.";

        public VoucherApplicableValidation()
        {
            RuleFor(c => c.Code)
                .NotEmpty()
                .WithMessage(CodeErroMsg);

            RuleFor(c => c.ExpirationDate)
                .Must(ExpirationDateGreaterThanCurrentDate)
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

            When(f => f.VoucherDiscountType == VoucherDiscountType.Value, () =>
            {
                RuleFor(c => c.DiscountValue)
                .NotNull()
                .WithMessage(DiscountValueErroMsg)
                .GreaterThan(0)
                .WithMessage(DiscountValueErroMsg);

            });

            When(f => f.VoucherDiscountType == VoucherDiscountType.Percentage, () =>
            {
                RuleFor(c => c.DiscountPercentual)
                .NotNull()
                .WithMessage(DiscountPercentualErroMsg)
                .GreaterThan(0)
                .WithMessage(DiscountValueErroMsg);

            });
        }

        protected static bool ExpirationDateGreaterThanCurrentDate(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }
}
