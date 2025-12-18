namespace CommsManager.Core.Common;

public static class Guard
{
    public static void AgainstNull(object argument, string argumentName)
    {
        if (argument == null)
            throw new ArgumentNullException(argumentName);
    }

    public static void AgainstNullOrEmpty(string argument, string argumentName)
    {
        if (string.IsNullOrEmpty(argument))
            throw new ArgumentException($"{argumentName} не может быть null или пустым", argumentName);
    }

    public static void AgainstNegative(decimal number, string argumentName)
    {
        if (number < 0)
            throw new ArgumentException($"{argumentName} не может быть null или пустым", argumentName);
    }

    public static void AgainstPastDate(DateTime date, string argumentName)
    {
        if (date < DateTime.UtcNow)
            throw new ArgumentException($"{argumentName} не может быть null или пустым", argumentName);
    }
}