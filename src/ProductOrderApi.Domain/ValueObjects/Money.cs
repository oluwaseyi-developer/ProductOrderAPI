namespace ProductOrderApi.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; }
        public string Currency { get; } = "NGN";

        public Money(decimal amount, string currency = "NGN")
        {
            if (amount < 0)
                throw new ArgumentException("Money amount cannot be negative");

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty");

            Amount = amount;
            Currency = currency.ToUpper();
        }

        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot add money of different currencies");

            return new Money(a.Amount + b.Amount, a.Currency);
        }

        public static Money operator *(Money money, int multiplier)
        {
            return new Money(money.Amount * multiplier, money.Currency);
        }
    }
}
