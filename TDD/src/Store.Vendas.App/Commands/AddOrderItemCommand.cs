using FluentValidation;
using Store.Core.Messages;
using Store.Sales.Domain;

namespace Store.Sales.App.Commands
{
    public class AddOrderItemCommand : Command
    {
        public Guid ClientId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }

        public AddOrderItemCommand(Guid clientId, Guid productId, string name, int quantity, decimal unitValue)
        {
            ClientId = clientId;
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {
        public static string ClientIdErroMsg => "Id do cliente inválido";
        public static string ProductIdErroMsg => "Id do produto inválido";
        public static string NameErroMsg => "O nome do produto não foi informado";
        public static string QtdMaxErroMsg => $"A quantidade máxima de um item é {Order.MAX_UNITS_ITEM}";
        public static string QtdMinErroMsg => "A quantidade miníma de um item é 1";
        public static string ValueErroMsg => "O valor do item precisa ser maior que 0";

        public AddOrderItemValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage(ClientIdErroMsg);

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErroMsg);

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(NameErroMsg);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QtdMinErroMsg)
                .LessThanOrEqualTo(Order.MAX_UNITS_ITEM)
                .WithMessage(QtdMaxErroMsg);

            RuleFor(c => c.UnitValue)
                .GreaterThan(0)
                .WithMessage(ValueErroMsg);
        }
    }
}