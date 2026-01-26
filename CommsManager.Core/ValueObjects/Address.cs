namespace CommsManager.Core.ValueObjects;

public sealed class Address(string street, string city, string state, string country, string zipCode) : ValueObject
{
    public string Street { get; } = street;
    public string City { get; } = city;
    public string State { get; } = state;
    public string Country { get; } = country;
    public string ZipCode { get; } = zipCode;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }

    public override string ToString() => $"{Street}, {City}, {State}, {Country} {ZipCode}";
}