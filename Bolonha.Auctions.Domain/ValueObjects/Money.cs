namespace Bolonha.Auctions.Domain.ValueObjects;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");

        Amount = amount;
    }

    public bool IsGreater(Money? other) 
        => Amount > other?.Amount;

    public bool Equals(Money? other)
       => Amount == other?.Amount;

    public override bool Equals(object? obj) 
        => obj is Money other && Equals(other);

    public override int GetHashCode() 
        => Amount.GetHashCode();

    public override string ToString() 
        => Amount.ToString("C");
}
