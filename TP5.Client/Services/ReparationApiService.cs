using System.Net.Http.Json;
using Blazored.SessionStorage;

namespace TP5.Client.Services;

public class ReparationApiService : IReparationApiService
{
    private readonly HttpClient _httpClient;

    // pour lire le token de la connexion
    private readonly ISessionStorageService _sessionStorage;

    public ReparationApiService(HttpClient httpClient, ISessionStorageService sessionStorage)
    {
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }

    // historique des reparations
    public async Task<List<HistoriqueReparationRecord>?> GetHistoriqueAsync()
    {
        var token = await _sessionStorage.GetItemAsStringAsync("authToken");
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        // request avec le token dans le header
        var request = new HttpRequestMessage(HttpMethod.Get, "api/Reparations/historique");
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<List<HistoriqueReparationRecord>>();
    }
}