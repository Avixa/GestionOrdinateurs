namespace TP5.Client.Services;

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // demande un token pour ce login
    public async Task<string?> GetTokenAsync(string login)
    {
        var response = await _httpClient.PostAsync($"api/Authentication?login={login}", null);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        // l'API renvoie directement le token en txt
        return await response.Content.ReadAsStringAsync();
    }
}