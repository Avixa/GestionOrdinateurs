using System.Net.Http.Json;
using Blazored.SessionStorage;
using TP5.Shared;

namespace TP5.Client.Services;

public class OrdinateurApiService : IOrdinateurApiService
{
    private readonly HttpClient _httpClient;

    // pour lire le token de la connexion
    private readonly ISessionStorageService _sessionStorage;

    public OrdinateurApiService(HttpClient httpClient, ISessionStorageService sessionStorage)
    {
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }

    // liste des ordinateurs
    public async Task<List<Ordinateur>?> GetOrdinateursAsync()
    {
        var token = await _sessionStorage.GetItemAsStringAsync("authToken");
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        // request avec le token dans le header
        var request = new HttpRequestMessage(HttpMethod.Get, "api/Ordinateurs");
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<List<Ordinateur>>();
    }
}