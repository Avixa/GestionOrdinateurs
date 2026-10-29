namespace TP5.Client.Services;

// call AuthentificationApi
public interface IAuthApiService
{
    Task<string?> GetTokenAsync(string login);
}