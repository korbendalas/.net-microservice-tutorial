using FluentValidation;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketResult(bool IsSuccess);

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName cannot be empty");
    }
}

public class StoreBasketHandler
{
    
}