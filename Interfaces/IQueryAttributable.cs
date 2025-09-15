namespace CommsManager.Interfaces;

public interface IQueryAttributable
{
    void ApplyQueryAttributes(IDictionary<string, object> query);
}

public interface IAsyncInitialize
{
    Task InitializeAsync();
}
