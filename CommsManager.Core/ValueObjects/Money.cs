namespace CommsManager.Core.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "RUB")
    {
        if (amount < 0)
            throw new ArgumentException("Сумма не может быть отрицательной");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Требуется валюта");

        Amount = amount;
        Currency = currency.ToUpper();
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Невозможно добавить деньги в разных валютах");

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Невозможно вычесть деньги в разных валютах");

        return new Money(left.Amount - right.Amount, left.Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:N2} {Currency}";

    public static Money FromString(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Входная строка не может быть пустой или null", nameof(input));

        var parts = input.Trim().Split([' '], StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
            throw new ArgumentException("Строка должна содержать сумму и валюту, разделённые пробелом", nameof(input));

        var amountStr = parts[0];
        var currency = parts[1];

        if (!decimal.TryParse(amountStr, System.Globalization.NumberStyles.Number,
            System.Globalization.CultureInfo.InvariantCulture, out decimal amount))
        {
            throw new ArgumentException($"Не удалось преобразовать '{amountStr}' в число", nameof(input));
        }

        if (amount < 0)
            throw new ArgumentException("Сумма не может быть отрицательной", nameof(input));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Валюта не может быть пустой", nameof(input));

        return new Money(amount, currency);
    }
}